using System.Collections.Generic;
using RMC.Core.Observables;
using RMC.Mini.Model;
using RMC.Mini.Samples.UGS.Mini.View;

namespace RMC.Mini.Samples.UGS.Mini.Model
{
    /// <summary>
    /// The Model stores runtime data 
    /// </summary>
    public class UserGeneratedContentModel: BaseModel // Extending 'base' is optional
    {
        //  Properties  ------------------------------------
        public Observable<Dictionary<string,string>> UserGeneratedContentItems { get { return _userGeneratedContentItems;} }
        public Observable<UgsListViewEntry> SelectedListItem { get { return _selectedListItem;} }

        public bool HasSelectedListItem
        {
            get
            {
                return SelectedListItem.Value != null;
            }
        }

        //  Fields ----------------------------------------
        private readonly Observable<Dictionary<string,string>> _userGeneratedContentItems = new Observable<Dictionary<string,string>> ();
        private readonly Observable<UgsListViewEntry> _selectedListItem = new Observable<UgsListViewEntry>();
        
        
        //  Initialization  -------------------------------
        public override void Initialize(IContext context) 
        {
            if (!IsInitialized)
            {
                base.Initialize(context);

                // Set Defaults
                _userGeneratedContentItems.Value = new Dictionary<string,string>();
                _selectedListItem.Value = null;
            }
        }

        //  Methods ---------------------------------------

        //  Event Handlers --------------------------------
    }
}