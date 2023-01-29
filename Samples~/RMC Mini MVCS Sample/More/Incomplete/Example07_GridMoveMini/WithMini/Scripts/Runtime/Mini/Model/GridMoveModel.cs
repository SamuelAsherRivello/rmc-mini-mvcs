using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Model;

namespace RMC.Core.Architectures.Mini.Samples.GridMove.WithMini.Mini.Model
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The Model stores runtime data 
    /// </summary>
    public class GridMoveModel: BaseModel // Extending 'base' is optional
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public Observable<bool> GridCellIndexIsPending { get { return _gridCellIndexIsPending;} }
        public Observable<int> GridCellIndex { get { return _gridCellIndex;} }
        public Observable<string> StatusText { get { return _statusText;} }
        
        //  Fields ----------------------------------------
        private readonly Observable<bool> _gridCellIndexIsPending = new Observable<bool>();
        private readonly Observable<int> _gridCellIndex = new Observable<int>();
        private readonly Observable<string> _statusText = new Observable<string>();
        
        //  Initialization  -------------------------------
        public override void Initialize(IContext context) 
        {
            if (!IsInitialized)
            {
                base.Initialize(context);
                
                // Set Defaults
                _gridCellIndexIsPending.Value = false;
                _gridCellIndex.Value = 0;
                _statusText.Value = "";
            }
        }
        
        //  Methods ---------------------------------------


        //  Event Handlers --------------------------------

    }
}