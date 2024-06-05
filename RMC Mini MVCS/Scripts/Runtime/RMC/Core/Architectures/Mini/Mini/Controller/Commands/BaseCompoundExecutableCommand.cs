
using System.Collections.Generic;
using RMC.Core.Architectures.Mini.Context;

namespace RMC.Core.Architectures.Mini.Controller.Commands
{
    /// <summary>
    /// This <see cref="IExecutableCommand"/> operates on
    /// a list of <see cref="IExecutableCommand"/> sub-instances
    /// like a macro.
    /// </summary>
    public class BaseCompoundExecutableCommand : IExecutableCommand
    {
        //  Properties ------------------------------------
        public virtual bool CanUndo { get { return true;} }
        protected List<IExecutableCommand> Commands { get { return _commands;} }
        
        //  Fields ----------------------------------------
        private List<IExecutableCommand> _commands = new List<IExecutableCommand>();
        
        //  Initialization  -------------------------------
        
        //  Methods ---------------------------------------
        public virtual bool Execute(IContext context)
        {
            bool didAllExecute = Commands.Count > 0;
            foreach (IExecutableCommand command in Commands)
            {
                if (!command.Execute(context))
                {
                    didAllExecute = false;
                }
            }
            return didAllExecute;
        }

        public virtual bool Undo(IContext context)
        {
            bool didAllUndo = Commands.Count > 0;
            foreach (IExecutableCommand command in Commands)
            {
                if (!command.Undo(context))
                {
                    didAllUndo = false;
                }
            }
            return didAllUndo;
        }

    }
}