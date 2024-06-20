namespace RMC.Mini.Experimental.Module
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The MiniMvcsWithModules is the parent object containing
    /// all <see cref="IConcern"/>s as children. It
    /// defines one instance of the Mvcs architectural
    /// framework within an application.
    ///
    /// It is compatible with <see cref="IModule"/>
    /// </summary>
    public class BaseModule: MiniMvcsWithModules
    {
        //  Events ----------------------------------------

        //  Properties ------------------------------------

        //  Fields ----------------------------------------

        //  Initialization  -------------------------------
        public BaseModule(Context context)
        {
            _context = context;
        }
        
        //  Methods ---------------------------------------

        //  Event Handlers --------------------------------
    }
}