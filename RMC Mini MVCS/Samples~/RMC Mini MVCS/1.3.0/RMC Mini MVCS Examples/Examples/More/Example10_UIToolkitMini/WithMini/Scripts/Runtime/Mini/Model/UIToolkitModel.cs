using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Model;

namespace RMC.Core.Architectures.Mini.Samples.Login.WithMini.Mini.Model
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The Model stores runtime data 
    /// </summary>
    public class UIToolkitModel: BaseModel // Extending 'base' is optional
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public Observable<int> HairIndex { get { return _hairIndex;} }
        public Observable<int> FaceIndex { get { return _faceIndex;} }
        public Observable<int> ShirtIndex { get { return _shirtIndex;} }
        public Observable<int> BodyIndex { get { return _bodyIndex;} }
        public CharacterData InitialCharacterData { get; set; }

        //  Fields ----------------------------------------
        private readonly Observable<int> _hairIndex = new Observable<int>();
        private readonly Observable<int> _faceIndex = new Observable<int>();
        private readonly Observable<int> _shirtIndex = new Observable<int>();
        private readonly Observable<int> _bodyIndex = new Observable<int>();

        
        //  Initialization  -------------------------------
        public override void Initialize(IContext context) 
        {
            if (!IsInitialized)
            {
                base.Initialize(context);

                // Set Defaults
                _hairIndex.Value = 0;
                _faceIndex.Value = 0;
                _shirtIndex.Value = 0;
                _bodyIndex.Value = 0;
            }
        }
        
        //  Methods ---------------------------------------


        //  Event Handlers --------------------------------

    }
}