using RMC.Core.Architectures.Mini.Controller.Commands;

namespace RMC.Core.Architectures.Mini.Context
{
    /// <summary>
    /// See <see cref="Context"/>
    /// </summary>
    public interface IContext
    {
        ModelLocator ModelLocator { get; }
        ICommandManager CommandManager { get; }
    }
}