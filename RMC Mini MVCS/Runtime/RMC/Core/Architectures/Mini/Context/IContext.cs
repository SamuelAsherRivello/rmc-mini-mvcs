using System;
using RMC.Core.Architectures.Mini.Controller.Commands;

namespace RMC.Core.Architectures.Mini.Context
{
    /// <summary>
    /// See <see cref="Context"/>
    /// </summary>
    public interface IContext : IDisposable
    {
        ModelLocator ModelLocator { get; }
        ICommandManager CommandManager { get; }
    }
}