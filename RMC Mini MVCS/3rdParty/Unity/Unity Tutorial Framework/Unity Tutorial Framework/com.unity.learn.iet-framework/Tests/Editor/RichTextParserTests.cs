using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Unity.Tutorials.Core.Editor;
using static Unity.Tutorials.Core.Editor.RichTextParser;

namespace Unity.Tutorials.Core.Editor.Tests
{
    public class RichTextParserTests
    {
        TutorialWindow m_Window;
        bool originalValueOfShowTutorialsWindowClosedDialog;

        string k_LoremIpsum = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. " +
            "Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown" +
            " printer took a galley of type and scrambled it to make a type specimen book. It has survived" +
            " not only five centuries, but also the leap into electronic typesetting, remaining essentially" +
            " unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem" +
            " Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including" +
            " versions of Lorem Ipsum.";

        int m_BoldUsed;
        int m_ItalicUsed;
        int m_LinksUsed;
        int m_NumParagraphs;
        int m_StyledUsed;

        string Bold(string toBold)
        {
            m_BoldUsed++;
            return " <b>" + toBold + "</b> ";
        }

        string Italic(string toBold)
        {
            m_ItalicUsed++;
            return " <i>" + toBold + "</i> ";
        }

        string Link(string URL, string Text)
        {
            m_LinksUsed++;
            return " <a href=" + '\"' + URL + '\"' + ">" + Text + "</a> ";
        }

        string EmptyParagraph()
        {
            m_NumParagraphs++;
            return "\n\n";
        }

        string Styled(string toStyled, string className)
        {
            m_StyledUsed++;
            return " <style class=" + '\"' + className + '\"' + ">" + toStyled + "</style>";
        }

        void Reset()
        {
            m_BoldUsed = 0;
            m_ItalicUsed = 0;
            m_LinksUsed = 0;
            m_NumParagraphs = 0;
        }

        [SetUp]
        public void SetUp()
        {
            Reset();
            m_Window = EditorWindow.GetWindow<TutorialWindow>();
            originalValueOfShowTutorialsWindowClosedDialog = TutorialFrameworkModel.s_ShowTutorialsWindowClosedDialog;
            TutorialFrameworkModel.s_ShowTutorialsWindowClosedDialog.SetValue(false);
            m_Window.rootVisualElement.style.flexDirection = FlexDirection.Row;
            m_Window.rootVisualElement.style.flexWrap = Wrap.Wrap;
        }

        [TearDown]
        public void TearDown()
        {
            TutorialFrameworkModel.s_ShowTutorialsWindowClosedDialog.SetValue(originalValueOfShowTutorialsWindowClosedDialog);
            m_Window.Close();
        }

        [Test]
        public void CanCreateWrappingTextLabels()
        {
            Reset();
            RichTextToVisualElements(CreateRichText(50, 0, 0, 0), m_Window.rootVisualElement);

            Assert.IsTrue(DoStylesMatch());
        }

        [Test]
        [TestCase("There are five words here", "teststyle", 5)]
        [TestCase("There are now six words here", "anotherteststyle", 6)]
        [TestCase("There are now eleven words here and it should be enough", "anotherteststyle", 11)]
        public void CanUseCustomStyleClasses(string testText, string styleClassName, int styledWords)
        {
            Reset();
            string testRichText = Styled(testText, styleClassName) + " " + testText;
            var visualElements = RichTextToVisualElements(testRichText, m_Window.rootVisualElement);
            Assert.AreEqual(visualElements.FindAll(elem => elem.ClassListContains(styleClassName)).Count, styledWords);
        }

        [Test]
        public void CanCreateRichText()
        {
            Reset();
            RichTextToVisualElements(CreateRichText(50, 10, 10, 5), m_Window.rootVisualElement);
            Assert.IsTrue(DoStylesMatch());
        }

        [Test]
        [TestCase("<b>asdf</b>asdf and some non bold words. <i>These are italic</i>", 1, 3, 6)]
        [TestCase("<b>bold words</b> and some non bold words. <i>These are italic</i>", 2, 3, 5)]
        [TestCase("<b>bold words</b>and some non bold words. <i>These are italic</i>", 2, 3, 5)]
        // [TestCase("<b>bold words</b>and some non bold words.<i>These are italic</i>", 2, 3, 5)] This fails. Todo: split anything from before <i> from the italic tag
        public void CanRenderRichTextPrecisely(string testRichText, int boldWords, int italicWords, int normalWords)
        {
            Reset();
            var visualElements = RichTextToVisualElements(testRichText, m_Window.rootVisualElement);
            Assert.AreEqual(visualElements.FindAll(elem => elem is BoldLabel).Count, boldWords);
            Assert.AreEqual(visualElements.FindAll(elem => elem is ItalicLabel).Count, italicWords);
            Assert.AreEqual(visualElements.FindAll(elem => !(elem is ItalicLabel) && !(elem is BoldLabel)).Count, normalWords);
        }

        [Test]
        [TestCase("<b>asdf</b>asdf", 1, 1, "asdf")]
        [TestCase("asdf<b>asdf</b>asdf", 1, 2, "asdf")]
        public void TestForTagParsingError(string testRichText, int boldWords, int normalWords, string wordToLookFor)
        {
            Reset();
            var visualElements = RichTextToVisualElements(testRichText, m_Window.rootVisualElement);
            foreach (VisualElement elem in visualElements)
            {
                Label label = (Label)elem;
                Assert.AreEqual(wordToLookFor, label.text);
            }
            Assert.AreEqual(boldWords + normalWords, visualElements.Count);
            Assert.AreEqual(boldWords, visualElements.FindAll(elem => elem is BoldLabel).Count);
            Assert.AreEqual(normalWords, visualElements.FindAll(elem => elem is TextLabel).Count);
        }

        [Test]
        [TestCase("      <b>bold words</b>and some non bold words. <i>These are italic</i>", 2, 3, 5, 6)]
        [TestCase("   Here are five normal words <i>And the next six are italic</i>", 0, 6, 5, 3)]
        public void LeadingWhiteSpaceIsPreserved(string testRichText, int boldWords, int italicWords, int normalWords, int whiteSpaceAmount)
        {
            Reset();
            var visualElements = RichTextToVisualElements(testRichText, m_Window.rootVisualElement);
            Assert.AreEqual(boldWords, visualElements.FindAll(elem => elem is BoldLabel).Count);
            Assert.AreEqual(italicWords, visualElements.FindAll(elem => elem is ItalicLabel).Count);
            Assert.AreEqual(normalWords, visualElements.FindAll(elem => elem is TextLabel).Count);
            WhiteSpaceLabel whitespacelabel = (WhiteSpaceLabel)visualElements.Find(elem => elem is WhiteSpaceLabel);
            Assert.AreEqual(whiteSpaceAmount, whitespacelabel.text.Length);
        }

        [Test]
        [TestCase("      <b>bold words</b>and some non bold words.<i>These are italic</i>         \n  a aa<b>a</b>                            ", 3, 3, 7, 6)]
        [TestCase("      <b>bold words</b>and some non bold words.<i>These are italic</i>         \n  a aa<b>a</b>", 3, 3, 7, 6)]
        public void TrailingWhiteSpaceIsRemoved(string testRichText, int boldWords, int italicWords, int normalWords, int whiteSpaceAmount)
        {
            Reset();
            var visualElements = RichTextToVisualElements(testRichText, m_Window.rootVisualElement);
            Assert.AreEqual(boldWords, visualElements.FindAll(elem => elem is BoldLabel).Count);
            Assert.AreEqual(italicWords, visualElements.FindAll(elem => elem is ItalicLabel).Count);
            Assert.AreEqual(normalWords, visualElements.FindAll(elem => elem is TextLabel).Count);
            WhiteSpaceLabel whitespacelabel = (WhiteSpaceLabel)visualElements.Find(elem => elem is WhiteSpaceLabel);
            Assert.AreEqual(whiteSpaceAmount, whitespacelabel.text.Length);
        }

        [Test]
        [TestCase("&amp;",1)]
        [TestCase("&amp; &lt;&gt;", 3)]
        [TestCase("Test&amp;Result", 11)]
        [TestCase("Test&amp;<b>Result</b>", 11)]
        [TestCase("Test &amp; <b>Result</b>", 11)]
        public void HTMLCharacterSupport (string testRichText, int totalCharacterCount)
        {
            Reset();
            var visualElements = RichTextToVisualElements(testRichText, m_Window.rootVisualElement);
            int totalCharacters = 0;
            foreach (Label label in visualElements)
            {
                totalCharacters += label.text.Length;
            }
            Assert.AreEqual(totalCharacterCount, totalCharacters);
        }

        [Test]
        public void CanCreateParagraphsOfRichText()
        {
            Reset();
            string richText = "";
            for (int i = 0; i < 10; i++)
            {
                richText += CreateRichText(i * 10, i * 2, i, i) + EmptyParagraph();
            }
            RichTextToVisualElements(richText, m_Window.rootVisualElement);
            Assert.IsTrue(DoStylesMatch());
        }

        bool DoStylesMatch()
        {
            int boldFound = CountStyles(typeof(BoldLabel));
            int italicFound = CountStyles(typeof(ItalicLabel));

            if (boldFound != m_BoldUsed)
            {
                Debug.LogError("Invalid amount of bold words. Entered: " + m_BoldUsed + " - Found: " + boldFound);
                return false;
            }

            if (italicFound != m_ItalicUsed)
            {
                Debug.LogError("Invalid amount of italic words. Entered: " + m_ItalicUsed + " - Found: " + italicFound);
                return false;
            }
            return true;
        }

        int CountWordLabels()
        {
            return m_Window.rootVisualElement.childCount;
        }

        int CountStyles(System.Type styleType)
        {
            var root = m_Window.rootVisualElement;
            int styledWords = 0;
            for (int i = 0; i < root.childCount; i++)
            {
                VisualElement element = root.ElementAt(i);

                if (element.GetType() == styleType)
                {
                    styledWords++;
                }
            }
            return styledWords;
        }

        string CreateRichText(int normalWords, int boldedWords, int italicWords, int links)
        {
            string richText = "";
            int wordNumber = 0;
            string[] loremIpsums = k_LoremIpsum.Split(' ');
            while (normalWords > 0 || boldedWords > 0 || italicWords > 0 || links > 0)
            {
                if (normalWords > 0)
                {
                    richText += loremIpsums[wordNumber] + " ";
                    normalWords--;
                    wordNumber = (wordNumber + 1) % loremIpsums.Length;
                }
                if (boldedWords > 0)
                {
                    richText += Bold(loremIpsums[wordNumber]);
                    boldedWords--;
                    wordNumber = (wordNumber + 1) % loremIpsums.Length;
                }
                if (italicWords > 0)
                {
                    richText += Italic(loremIpsums[wordNumber]);
                    italicWords--;
                    wordNumber = (wordNumber + 1) % loremIpsums.Length;
                }
                if (links > 0)
                {
                    richText += Link("http://www.google.fi", loremIpsums[wordNumber]);
                    links--;
                    wordNumber = (wordNumber + 1) % loremIpsums.Length;
                }
            }
            return richText;
        }
    }
}
