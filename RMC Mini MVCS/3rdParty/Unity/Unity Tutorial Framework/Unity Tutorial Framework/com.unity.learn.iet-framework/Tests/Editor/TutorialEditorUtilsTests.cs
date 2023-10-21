using NUnit.Framework;

namespace Unity.Tutorials.Core.Editor.Tests
{
    public class TutorialEditorUtilsTests
    {
        [TestCase("unity.com", ExpectedResult = "unity.com", TestName = "Plain 2nd domain level URL")]
        [TestCase("play.unity.com", ExpectedResult = "play.unity.com", TestName = "Plain 3rd domain level URL")]
        [TestCase("pLAy.unITy.cOm", ExpectedResult = "pLAy.unITy.cOm", TestName = "Plain 3rd domain level URL with caps")]
        [TestCase("http://unity.com", ExpectedResult = "unity.com", TestName = "2nd domain level URL with HTTP")]
        [TestCase("http://play.unity.com", ExpectedResult = "play.unity.com", TestName = "3rd domain level URL with HTTP")]
        [TestCase("https://unity.com", ExpectedResult = "unity.com", TestName = "2nd domain level URL with HTTPS")]
        [TestCase("https://play.unity.com", ExpectedResult = "play.unity.com", TestName = "3rd domain level URL with HTTPS")]
        [TestCase("htTpS://plAY.unITy.cOm", ExpectedResult = "plAY.unITy.cOm", TestName = "3rd domain level URL with HTTPS and caps")]
        public string RemoveHTTPProtocolPrefix_AlwaysFiltersURLProperly(string url)
        {
            return TutorialEditorUtils.RemoveHttpProtocolPrefix(url);
        }

        [Test]
        [TestCase("Test \r More testing \r And some more \n", "Test  More testing  And some more \n")]
        [TestCase("Test \n \n More testing \r \t And some more \n", "Test \n \n More testing  \t And some more \n")]
        public void SanitiseSpecialCharacters(string toSanitize, string expectedResult)
        {
            Assert.AreEqual(POFileUtils.SanitizeString(toSanitize), expectedResult);
        }

        [TestCase("unity.com", ExpectedResult = true, TestName = "Plain 2nd domain level URL")]
        [TestCase("play.unity.com", ExpectedResult = true, TestName = "Plain 3rd domain level URL")]
        [TestCase("plAY.unITy.cOm/index.php", ExpectedResult = true, TestName = "Extended 3rd domain level URL with caps")]
        [TestCase("http://unity.com", ExpectedResult = true, TestName = "2nd domain level URL with HTTP")]
        [TestCase("https://play.unity.com", ExpectedResult = true, TestName = "3rd domain level URL with HTTPS")]
        [TestCase("htTpS://plAY.unITy.cOm", ExpectedResult = true, TestName = "3rd domain level URL with HTTPS and caps")]
        [TestCase("http://play.unity.com/test.index", ExpectedResult = true, TestName = "Extended 3rd domain level URL")]
        [TestCase("https://unity.com/test.index", ExpectedResult = true, TestName = "2nd domain level URL with HTTPS")]
        [TestCase("https://docs.google.com/best-unity-doc.index", ExpectedResult = false, TestName = "3rd domain level URL with HTTPS")]
        [TestCase("https://docs.google.com/best.unity.doc.index", ExpectedResult = false, TestName = "3rd domain level URL with HTTPS")]
        public bool IsUnityUrlRequiringAuthentication_ReturnsTrueForUnityURLs(string url)
        {
            return TutorialEditorUtils.IsUnityUrlRequiringAuthentication(url);
        }
    }
}
