using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Unity.Tutorials.Core.Editor
{
    /// <summary>
    /// Good info on PO: http://pology.nedohodnik.net/doc/user/en_US/ch-poformat.html
    /// </summary>
    internal static class POFileUtils
    {
        /// <summary>
        /// Entries that need to be translated will have this ocmment
        /// </summary>
        const string k_TranslatorCommentForAskingLocalization = "TODO: translate";
        static readonly string s_IETLocalizationFilesFolder = $"Packages/{FrameworkSettings.k_PackageName}/Editor/Localization/";

        /// <summary>
        /// Currently supported languages, in addition to English.
        /// </summary>
        public static readonly Dictionary<SystemLanguage, string> SupportedLanguages = new Dictionary<SystemLanguage, string>
        {
            { SystemLanguage.Japanese, "ja" },
            { SystemLanguage.Korean, "ko" },
            { SystemLanguage.ChineseSimplified, "zh-hans" },
            { SystemLanguage.ChineseTraditional, "zh-hant" },
        };

        /// <summary>
        /// Creates a PO file header.
        /// https://www.gnu.org/software/trans-coord/manual/gnun/html_node/PO-Header.html
        /// https://www.gnu.org/software/gettext/manual/html_node/Header-Entry.html
        /// </summary>
        /// <param name="langCode"></param>
        /// <param name="name"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public static string CreateHeader(string langCode, string name, string version) =>
            // NOTE We don't have POTs so for POT-Creation-Date I just picked something.
            // TODO Value of Plural-Forms not probably true for all languages we support?
            // TODO check if we want to fill something more to the header
            $@"
msgid """"
msgstr """"
""Project-Id-Version: {name}@{version} \n""
""Report-Msgid-Bugs-To: \n""
""Language-Team: #devs-localization\n""
""POT-Creation-Date: 2020-05-15 21:02+03:00\n""
""PO-Revision-Date: {DateTime.Now.ToString(DateTimeFormat)}\n""
""Language: {langCode}\n""
""MIME-Version: 1.0\n""
""Content-Type: text/plain; charset=UTF-8\n""
""Content-Transfer-Encoding: 8bit\n""
""Plural-Forms: nplurals=2; plural=(n != 1);\n""
""X-Generator: com.unity.learn.iet-framework.authoring\n""
";

        /// <summary>
        /// Using the format given here https://www.gnu.org/software/trans-coord/manual/gnun/html_node/PO-Header.html
        /// </summary>
        public const string DateTimeFormat = "yyyy-MM-dd HH:mmK";

        /// <summary>
        /// http://pology.nedohodnik.net/doc/user/en_US/ch-poformat.html, "2.3.3. Escape Sequences"
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string EscapeString(string str)
        {
            // adapted from https://stackoverflow.com/a/14502246
            if (str.IsNullOrEmpty())
                return str;
            var literal = new StringBuilder(str.Length);
            foreach (var c in str)
            {
                if (k_EscapeMapping.ContainsKey(c))
                    literal.Append(k_EscapeMapping[c]);
                else if (char.GetUnicodeCategory(c) == UnicodeCategory.Control)
                    literal.Append($@"\u{c:x4}");
                else
                    literal.Append(c);
            }
            return literal.ToString();
        }

        /// <summary>
        /// Sanitizes, i.e., removes, problematic control characters, from a string that is used in a PO file.
        /// </summary>
        /// <param name="str"></param>
        /// <returns>String without unprintable control characters.</returns>
        public static string SanitizeString(string str)
        {
            return Regex.Replace(str, k_SanitizeRegexString, "");
        }

        static readonly Dictionary<char, string> k_EscapeMapping = new Dictionary<char, string>()
        {
            { '\"', "\\\"" },   // no need to sanitize
            { '\\', @"\\"},     // no need to sanitize
            { '\0', @"\0"},
            { '\a', @"\a"},
            { '\b', @"\b"},
            { '\f', @"\f"},
            { '\n', @"\n"},     // no need to sanitize
            { '\r', @"\r"},
            { '\t', @"\t"},     // no need to sanitize
            { '\v', @"\v"},
        };

        // Chars to sanitize are a subset of the chars to escape
        static readonly string k_SanitizeRegexString =
            string.Join("|", k_EscapeMapping.Keys.Except(new[] { '\"', '\\', '\n', '\t' }).ToArray());

        /// <summary>
        /// https://www.gnu.org/software/gettext/manual/html_node/PO-Files.html
        /// </summary>
        /// <remarks>
        /// The values are not escaped until they are serialized.
        /// </remarks>
        public class POEntry
        {
            /// <summary> # translator-comments </summary>
            public string TranslatorComments;
            /// <summary> #. extracted-comments  </summary>
            public string ExtractedComments;
            /// <summary> #: reference </summary>
            public string Reference;
            /// <summary> #, flag </summary>
            public string Flag;
            /// <summary> #| msgid "previous-untranslated-string" </summary>
            public string PreviousUntranslatedString;
            /// <summary> msgid "untranslated-string" </summary>
            public string ID;
            /// <summary> msgstr "translated-string" </summary>
            public string TranslatedString;

            /// <summary>
            /// Does this entry contain the minimum information to be a valid entry.
            /// </summary>
            /// <returns></returns>
            public bool IsValid() => Reference.IsNotNullOrEmpty() || ID.IsNotNullOrEmpty();

            /// <summary>
            /// Serializes this entry to a string representation.
            /// </summary>
            /// <remarks>
            /// All values will be escaped.
            /// </remarks>
            /// <returns></returns>
            public string Serialize()
            {
                return string.Format(
                    "{0}" +
                    "{1}" +
                    "{2}" +
                    "{3}" +
                    "{4}" +
                    "msgid \"{5}\"\n" +
                    "msgstr \"{6}\"",
                    TranslatorComments.IsNotNullOrEmpty() ? $"# {EscapeString(TranslatorComments)}\n" : string.Empty,
                    ExtractedComments.IsNotNullOrEmpty() ? $"#. {EscapeString(ExtractedComments)}\n" : string.Empty,
                    Reference.IsNotNullOrEmpty() ? $"#: {EscapeString(Reference)}\n" : string.Empty,
                    Flag.IsNotNullOrEmpty() ? $"#, {EscapeString(Flag)}\n" : string.Empty,
                    PreviousUntranslatedString.IsNotNullOrEmpty() ? $"#| {EscapeString(PreviousUntranslatedString)}" : string.Empty,
                    EscapeString(ID),
                    EscapeString(TranslatedString)
                );
            }
        }

        /// <summary>
        /// Reads a PO file and creates a list of PO entries.
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        // TODO Currently unused, implement a unit test at minimum if wanting to keep this around or make internal or remove.
        public static List<POEntry> ReadPOFile(string filepath)
        {
            const string str = "msgstr ";
            const string id = "msgid ";
            const string previd = "#| msgstr ";
            const string flag = "#,";
            const string reference = "#:";
            const string ecomment = "#.";
            const string tcomment = "#";

            var ret = new List<POEntry>();
            try
            {
                using (var fileStream = new FileStream(filepath, FileMode.Open, FileAccess.Read))
                using (var streamReader = new StreamReader(fileStream, Utf8WithoutBom))
                {
                    var entry = new POEntry();
                    string line;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        if (line.StartsWith(str))
                        {
                            entry.TranslatedString = line.Substring(str.Length);
                            entry.TranslatedString = entry.TranslatedString.Trim(new char[] { ' ', '\"' });
                        }
                        if (line.StartsWith(id))
                        {
                            entry.ID = line.Substring(id.Length);
                            entry.ID = entry.ID.Trim(new char[] { ' ', '\"' });
                        }
                        if (line.StartsWith(previd))
                        {
                            entry.PreviousUntranslatedString = line.Substring(previd.Length);
                            entry.PreviousUntranslatedString = entry.PreviousUntranslatedString.Trim(new char[] { ' ', '\"' });
                        }
                        if (line.StartsWith(flag)) entry.Flag = line.Substring(flag.Length).Trim();
                        if (line.StartsWith(reference)) entry.Reference = line.Substring(reference.Length).Trim();
                        if (line.StartsWith(ecomment)) entry.ExtractedComments = line.Substring(ecomment.Length).Trim();
                        if (line.StartsWith(tcomment)) entry.TranslatorComments = line.Substring(tcomment.Length).Trim();

                        if (line.IsNullOrWhiteSpace() && entry.IsValid())
                        {
                            ret.Add(entry);
                            entry = new POEntry();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

            return ret;
        }

        /// <summary>
        /// Writes a PO file.
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="projectVersion"></param>
        /// <param name="langCode"></param>
        /// <param name="entries"></param>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public static bool WritePOFile(string projectName, string projectVersion, string langCode, IEnumerable<POEntry> entries, string filepath)
        {
            try
            {
                using (var sw = new StreamWriter(filepath, append: false, Utf8WithoutBom))
                {
                    sw.Write(CreateHeader(langCode, projectName, projectVersion));
                    // Editor's handling of PO files seems very finicky, an empty line after the header
                    // and before the first entry required.
                    sw.WriteLine();
                    foreach (var entry in entries)
                    {
                        sw.WriteLine(entry.Serialize());
                        sw.WriteLine();
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return false;
            }
        }

        // Let's be very explicit about this, using e.g. System.Text.Encoding.UTF8 gives UTF-8 with BOM...
        static UTF8Encoding Utf8WithoutBom => new UTF8Encoding();

        internal static void ConvertIETLocalizationFileFromV2ToV3(string languageCode)
        {
            (List<POEntry> englishEntries, List<POEntry> targetLanguageEntries) = GetSourceAndTargetLanguageExistingEntries("en", languageCode);

            var outputEntries = new List<POEntry>();
            foreach (var englishEntry in englishEntries)
            {
                POEntry foundEntry = targetLanguageEntries.Find(e => e.ID == englishEntry.TranslatedString);
                POEntry resultEntry = new POEntry() { ID = englishEntry.ID };
                if (foundEntry == null)
                {
                    resultEntry.TranslatedString = englishEntry.TranslatedString;
                    englishEntry.TranslatorComments = k_TranslatorCommentForAskingLocalization;
                }
                else
                {
                    resultEntry.TranslatedString = foundEntry.TranslatedString;
                }
                outputEntries.Add(resultEntry);
            }

            WritePOFile(Application.productName, Application.version, languageCode, outputEntries, $"{s_IETLocalizationFilesFolder}{languageCode}.po");
        }

        static (List<POEntry>, List<POEntry>) GetSourceAndTargetLanguageExistingEntries(string sourceLanguageCode, string targetLanguageCode)
        {
            return (ReadPOFile($"{s_IETLocalizationFilesFolder}{sourceLanguageCode}.po"), ReadPOFile($"{s_IETLocalizationFilesFolder}{targetLanguageCode}.po"));
        }


        /// <summary>
        /// Syncs the entries of a non-english file with the ones of the english file, adding the ones that are not present in the non-english file
        /// </summary>
        /// <param name="languageCode"></param>
        internal static void SyncIETLocalizationFileWithEnglish(string languageCode)
        {
            (List<POEntry> englishEntries, List<POEntry> targetLanguageEntries) = GetSourceAndTargetLanguageExistingEntries("en", languageCode);

            var outputEntries = new List<POEntry>();
            foreach (var englishEntry in englishEntries)
            {
                string entryID = englishEntry.ID;
                POEntry foundEntry = targetLanguageEntries.Find(e => e.ID == entryID);
                POEntry resultEntry = new POEntry() { ID = entryID };
                if (foundEntry == null)
                {
                    resultEntry.TranslatedString = englishEntry.TranslatedString;
                    resultEntry.TranslatorComments = k_TranslatorCommentForAskingLocalization;
                }
                else
                {
                    if (foundEntry.TranslatorComments == k_TranslatorCommentForAskingLocalization)
                    {
                        resultEntry.TranslatedString = englishEntry.TranslatedString;
                        resultEntry.TranslatorComments = k_TranslatorCommentForAskingLocalization;
                    }
                    else
                    {
                        resultEntry.TranslatedString = foundEntry.TranslatedString;
                    }
                }
                outputEntries.Add(resultEntry);
            }

            WritePOFile(Application.productName, Application.version, languageCode, outputEntries, $"{s_IETLocalizationFilesFolder}{languageCode}.po");
        }
    }
}
