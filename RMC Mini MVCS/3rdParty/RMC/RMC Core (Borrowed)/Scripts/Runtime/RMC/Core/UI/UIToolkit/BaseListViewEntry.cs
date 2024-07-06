namespace RMC.Core.UI.UIToolkit
{
    /// <summary>
    /// Each item in the list
    /// </summary>
    public class BaseListViewEntry<T> where T : class
    {
        //  Properties ------------------------------------

        //  Fields ----------------------------------------
        public string Title;
        public T Data;
        
        //  Initialization  -------------------------------
        public BaseListViewEntry(string title, T data)
        {
            Title = title;
            Data = data;
        }
        
        //  Unity Methods ---------------------------------

        //  Methods ---------------------------------------
    }
}