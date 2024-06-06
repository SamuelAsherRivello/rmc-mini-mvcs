using RMC.Core.Architectures.Mini.Samples.Configurator.Standard;
using RMC.Core.Architectures.Mini.Samples.Configurator.Standard.Gameplay;
using UnityEngine;

namespace RMC.Core.Architectures.Mini.Samples.Configurator.Mini.Model.Data
{
    /// <summary>
    /// Defines the customizable characteristics for the <see cref="Environment"/>
    /// </summary>
    [System.Serializable]
    public class EnvironmentData : System.IEquatable<EnvironmentData>
    {
        //  Fields ------------------------------------
        public Color FloorColor = Color.white;
        public Color BackgroundColor = Color.white;
        public Color DecorationColor = Color.white;

        //  Methods ------------------------------------
        public static EnvironmentData FromRandomValues()
        {
            var colors = CustomColorUtility.GetRandomColorsWithoutRepeat(3);
            return new EnvironmentData
            {
                FloorColor = colors[0],
                BackgroundColor = colors[1],
                DecorationColor = colors[2]
            };
        }

        public static EnvironmentData FromDefaultValues()
        {
            var colors = CustomColorUtility.GetDefaultColorList();
            return new EnvironmentData
            {
                FloorColor = colors[0],
                BackgroundColor = colors[1],
                DecorationColor = colors[2]
            };
        }

        // Implementing IEquatable<EnvironmentData>
        public bool Equals(EnvironmentData other)
        {
            if (other == null) return false;
            return FloorColor.Equals(other.FloorColor) &&
                   BackgroundColor.Equals(other.BackgroundColor);
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals(obj as EnvironmentData);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + FloorColor.GetHashCode();
                hash = hash * 23 + BackgroundColor.GetHashCode();
                return hash;
            }
        }

        public static bool operator ==(EnvironmentData left, EnvironmentData right)
        {
            if (ReferenceEquals(left, null)) return ReferenceEquals(right, null);
            return left.Equals(right);
        }

        public static bool operator !=(EnvironmentData left, EnvironmentData right)
        {
            return !(left == right);
        }
    }
}
