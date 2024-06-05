
namespace RMC.Core.Templates
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>
    public class ClassTemplate
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public string SamplePublicText { get { return _samplePublicText;} set { _samplePublicText = value;}}

        
        //  Fields ----------------------------------------
        private string _samplePublicText;

        
        //  Initialization  -------------------------------
        
        
        //  Unity Methods   -------------------------------
        protected void Start ()
        {
          
        }
        
        
        //  Other Methods ---------------------------------
        public string SamplePublicMethod (string message) 
        {
            return message;
        }
        
        
        //  Event Handlers --------------------------------
        private void Target_OnCompleted(string message) 
        {
            
        }

    }
}
