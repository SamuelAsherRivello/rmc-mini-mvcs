using System;
using RMC.Mini.Controller.Commands;
using RMC.Mini.Model;

//Keep As:RMC.Mini
namespace RMC.Mini
{
    /// <summary>
    /// See <see cref="Context"/>
    /// </summary>
    public interface IContext : IDisposable
    {
        Locator<IModel> ModelLocator { get; }
        ICommandManager CommandManager { get; }
    }
}