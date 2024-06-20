using System.Collections.Generic;
using RMC.Core.Observables;
using RMC.Mini.Model;
using RMC.Mini.Samples.UGS.Mini.View;
using Unity.Services.CloudSave.Models;

namespace RMC.Mini.Samples.UGS.Mini.Model
{
    /// <summary>
    /// The Model stores runtime data 
    /// </summary>
    public class CloudSaveModel: BaseModel // Extending 'base' is optional
    {
        //  Properties  -----------------------------------
        public Observable<Dictionary<string,Item>> CloudSaveItems { get { return _cloudSaveItems;} }
        public Observable<UgsListViewEntry> SelectedListItem { get { return _selectedListItem;} }

        //  Fields ----------------------------------------
        private readonly Observable<Dictionary<string,Item>> _cloudSaveItems = new Observable<Dictionary<string,Item>>();
        private readonly Observable<UgsListViewEntry> _selectedListItem = new Observable<UgsListViewEntry>();
        //  Initialization  -------------------------------
        public override void Initialize(IContext context) 
        {
            if (!IsInitialized)
            {
                base.Initialize(context);

                // Set Defaults
                _cloudSaveItems.Value = new Dictionary<string,Item>();
                _selectedListItem.Value = null;

            }
        }

        //  Methods ---------------------------------------

        //  Event Handlers --------------------------------
    }
}