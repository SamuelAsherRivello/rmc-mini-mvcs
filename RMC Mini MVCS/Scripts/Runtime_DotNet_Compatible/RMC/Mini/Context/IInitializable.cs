//Keep As:RMC.Mini
namespace RMC.Mini
{
    /// <summary>
    /// Enforces API for types which Initialize.
    /// </summary>
    public interface IInitializable
    {
        //  Properties  ------------------------------------
        public bool IsInitialized { get; }

        //  General Methods  ------------------------------
        public void Initialize();
        void RequireIsInitialized();
    }
}