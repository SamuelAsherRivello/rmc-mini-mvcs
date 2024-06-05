using System;
using RMC.Core.Architectures.Mini.Controller.Commands;
using RMC.Core.Architectures.Mini.Locators;
using RMC.Core.Architectures.Mini.Model;

//Keep as: RMC.Core.Architectures.Mini
namespace  RMC.Core.Architectures.Mini
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