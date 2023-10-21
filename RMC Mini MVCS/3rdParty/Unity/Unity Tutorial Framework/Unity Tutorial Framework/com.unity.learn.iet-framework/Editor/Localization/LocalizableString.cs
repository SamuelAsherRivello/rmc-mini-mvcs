using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Unity.Tutorials.Core.Editor
{
    /// <summary>
    /// A serializable string that is localized at run-time.
    /// </summary>
    [Serializable]
    public class LocalizableString : ISerializationCallbackReceiver
    {
        internal const string PropertyPath = "m_Untranslated";
        internal const string OldPropertyPath = "<Untranslated>k__BackingField";

        /// <summary>
        /// Setting Untranslated string overwrites Translated so make sure to translate again.
        /// </summary>
        public string Untranslated
        {
            get => m_Untranslated;
            set => Translated = m_Untranslated = value;
        }

        [SerializeField, FormerlySerializedAs(OldPropertyPath)]
        string m_Untranslated;

        /// <summary>
        /// The localized strings, if it exists.
        /// </summary>
        public string Translated { get; set; }
        /// <summary>
        /// The translated string, if exists, untranslated otherwise.
        /// </summary>
        public string Value => Translated.AsNullIfEmpty() ?? Untranslated;

        /// <summary>
        /// Default-constructs with empty strings.
        /// </summary>
        public LocalizableString() : this(string.Empty) {}
        /// <summary>
        /// Constructs with an untranslated string.
        /// </summary>
        /// <param name="untranslated"></param>
        public LocalizableString(string untranslated) { Untranslated = untranslated; }

        /// <summary>
        /// Implicitly constructs from an untranslated string.
        /// </summary>
        /// <param name="untranslated"></param>
        /// <returns></returns>
        public static implicit operator LocalizableString(string untranslated) => new LocalizableString(untranslated);
        /// <summary>
        /// Implicit conversion to string returns the Value.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static implicit operator string(LocalizableString str) => str.Value;

        /// <summary>
        /// UnityEngine.ISerializationCallbackReceiver override, do not call.
        /// </summary>
        public void OnBeforeSerialize() {}

        /// <summary>
        /// UnityEngine.ISerializationCallbackReceiver override, do not call.
        /// </summary>
        public void OnAfterDeserialize()
        {
            // Replicate the Untranslated setter behaviour upon deserialization.
            Translated = Untranslated;
        }
    }

    /// <summary>
    /// Same as TextAreaAttribute but used for LocalizableStrings.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class LocalizableTextAreaAttribute : PropertyAttribute
    {
        /// <summary>
        /// Minimum number of lines shown in the Inspector.
        /// </summary>
        public readonly int MinLines;
        /// <summary>
        /// Maximum number of lines shown in the Inspector.
        /// </summary>
        public readonly int MaxLines;

        /// <summary>
        /// Default-constructs with default (3) number lines.
        /// </summary>
        public LocalizableTextAreaAttribute()
        {
            MinLines = 3;
            MaxLines = 3;
        }

        /// <summary>
        /// Constructs with desired number of lines.
        /// </summary>
        /// <param name="minLines"></param>
        /// <param name="maxLines"></param>
        public LocalizableTextAreaAttribute(int minLines, int maxLines)
        {
            MinLines = minLines;
            MaxLines = maxLines;
        }
    }
}
