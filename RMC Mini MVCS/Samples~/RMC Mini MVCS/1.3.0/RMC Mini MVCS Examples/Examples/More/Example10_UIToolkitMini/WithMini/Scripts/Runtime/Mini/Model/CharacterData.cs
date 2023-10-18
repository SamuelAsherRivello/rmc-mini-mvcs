
namespace RMC.Core.Architectures.Mini.Samples.Login.WithMini.Mini.Model
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// Data transfer object (DTO) containing all
    /// info needed to request a login.
    /// </summary>
    public class CharacterData
    {
        //  Events ----------------------------------------

        //  Properties ------------------------------------
        public int HairIndex { get; set; }
        public int FaceIndex { get; set; }
        public int ShirtIndex { get; set; }
        public int BodyIndex { get; set; }
        
        //  Methods ---------------------------------------
        public override string ToString()
        {
            return $"[CharacterData ({HairIndex}, {FaceIndex}, {ShirtIndex}, {BodyIndex})]";
        }

        //  Event Handlers --------------------------------

    }
}