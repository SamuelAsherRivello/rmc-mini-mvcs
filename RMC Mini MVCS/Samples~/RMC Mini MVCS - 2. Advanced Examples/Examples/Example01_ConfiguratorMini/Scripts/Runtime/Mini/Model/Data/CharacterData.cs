using RMC.Core.Architectures.Mini.Samples.Configurator.Standard;
using RMC.Core.Architectures.Mini.Samples.Configurator.Standard.Gameplay;
using UnityEngine;

namespace RMC.Core.Architectures.Mini.Samples.Configurator.Mini.Model.Data
{
    /// <summary>
    /// Defines the customizable characteristics for the <see cref="Player"/>
    /// </summary>
    [System.Serializable]
    public class CharacterData : System.IEquatable<CharacterData>
    {
        //  Fields ------------------------------------
        public Color HeadColor = Color.white;
        public Color ChestColor = Color.white;
        public Color LegsColor = Color.white;

        //  Methods ------------------------------------
        public static CharacterData FromRandomValues()
        {
            var colors = CustomColorUtility.GetRandomColorsWithoutRepeat(3);
            return new CharacterData
            {
                HeadColor = colors[0],
                ChestColor = colors[1],
                LegsColor = colors[2],
            };
        }

        public static CharacterData FromDefaultValues()
        {
            var colors = CustomColorUtility.GetDefaultColorList();
            return new CharacterData
            {
                HeadColor = colors[0],
                ChestColor = colors[2],
                LegsColor = colors[4],
            };
        }

        // Implementing IEquatable<CharacterData>
        public bool Equals(CharacterData other)
        {
            if (other == null) return false;
            return HeadColor.Equals(other.HeadColor) &&
                   ChestColor.Equals(other.ChestColor) &&
                   LegsColor.Equals(other.LegsColor);
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals(obj as CharacterData);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + HeadColor.GetHashCode();
                hash = hash * 23 + ChestColor.GetHashCode();
                hash = hash * 23 + LegsColor.GetHashCode();
                return hash;
            }
        }

        public static bool operator ==(CharacterData left, CharacterData right)
        {
            if (ReferenceEquals(left, null)) return ReferenceEquals(right, null);
            return left.Equals(right);
        }

        public static bool operator !=(CharacterData left, CharacterData right)
        {
            return !(left == right);
        }
    }
}
