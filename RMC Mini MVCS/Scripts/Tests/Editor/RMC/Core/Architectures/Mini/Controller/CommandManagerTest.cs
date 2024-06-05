using NUnit.Framework;
using RMC.Core.Architectures.Mini.Controller.Commands;
using RMC.Core.Architectures.Mini.Model;
using RMC.Core.Data.Types;
using RMC.Core.Experimental;

namespace RMC.Core.Architectures.Mini.Controller
{
    [Category ("RMC.Mini")]
    public class CommandManagerTest
    {
        public class TestModel: BaseModel 
        {
            public Observable<int> Counter { get { return _counter;} }
            private readonly Observable<int> _counter = new Observable<int>();
        }
        
        public class TestCommand1 : ICommand
        {
        }
        
        public class TestCommand2 : ICommand
        {
        }
        
        public class TestExecutableCommand1 : IExecutableCommand
        {
            public bool CanUndo { get {return true;} }
            public bool Execute(IContext context)
            {
                return true;
            }

            public bool Undo(IContext context)
            {
                return true;
            }
        }
        
        public class TestExecutableCommandWithoutUndo : IExecutableCommand
        {
            public bool CanUndo { get {return false;} }
            public bool Execute(IContext context)
            {
                return true;
            }

            public bool Undo(IContext context)
            {
                return true;
            }
        }
        
        public class TestExecutableCommand2 : IExecutableCommand
        {
            public bool CanUndo { get {return true;} }
            public bool Execute(IContext context)
            {
                return true;
            }

            public bool Undo(IContext context)
            {
                return true;
            }
        }
        
        
        public class IncrementCounterExecutableCommand : IExecutableCommand
        {
            public bool CanUndo { get {return true;} }
                              
            public bool Execute(IContext context)
            {
                bool didExecute = false;
                if (context.ModelLocator.HasItem<TestModel>())
                {
                    TestModel testModel = context.ModelLocator.GetItem<TestModel>();
                    testModel.Counter.Value++;
                    didExecute = true;
                }
                return didExecute;
            }
                  
            public bool Undo(IContext context)
            {
                bool didUndo = false;
                if (context.ModelLocator.HasItem<TestModel>())
                {
                    TestModel testModel = context.ModelLocator.GetItem<TestModel>();
                    testModel.Counter.Value--;
                    didUndo = true;
                }
                return didUndo;
            }
        }

        public class IncrementCounter3CompoundExecutableCommand : BaseCompoundExecutableCommand
        {
            public IncrementCounter3CompoundExecutableCommand()
            {
                Commands.Add(new IncrementCounterExecutableCommand());
                Commands.Add(new IncrementCounterExecutableCommand());
                Commands.Add(new IncrementCounterExecutableCommand());
            }
        }
        
        [TearDown]
        public void TearDown()
        {
            if (ContextLocator.Instance.HasItem<Mini.Context>())
            {
                ContextLocator.Instance.RemoveItem<Mini.Context>();
            }
        }
        
        [Test]
        public void CommandWasCalled_IsTrue_WhenInvokeCommand()
        {
            // Arrange
            Mini.Context context = new Mini.Context();
            CommandManager commandManager = new CommandManager(context);
            bool commandWasCalled = false;
            commandManager.AddCommandListener<TestCommand1>(
                delegate(TestCommand1 command)
                {
                    commandWasCalled = true;
                });
            
            // Act
            commandManager.InvokeCommand(new TestCommand1());

            // Assert
            Assert.That(commandWasCalled, Is.True);
        }
        
        [Test]
        public void CommandWasCalled_IsFalse_WhenNotInvokeCommand()
        {
            // Arrange
            Mini.Context context = new Mini.Context();
            CommandManager commandManager = new CommandManager(context);
            bool commandWasCalled = false;
            commandManager.AddCommandListener<TestCommand1>(
                delegate(TestCommand1 command)
                {
                    commandWasCalled = true;
                });
            
            // Act
            commandManager.InvokeCommand(new TestCommand2());  //Sending 2, not 1

            // Assert
            Assert.That(commandWasCalled, Is.False);
        }
        
        [Test]
        public void ExecutableCommandWasCalled_IsTrue_WhenInvokeExecutableCommand()
        {
            // Arrange
            Mini.Context context = new Mini.Context();
            CommandManager commandManager = new CommandManager(context);
            bool commandWasCalled = false;
            commandManager.AddCommandListener<TestExecutableCommand1>(
                delegate(TestExecutableCommand1 command)
                {
                    commandWasCalled = true;
                });
            
            // Act
            commandManager.InvokeCommand(new TestExecutableCommand1());

            // Assert
            Assert.That(commandWasCalled, Is.True);
        }
        
        [Test]
        public void ExecutableCommandWasCalled_IsFalse_WhenNotInvokeExecutableCommand()
        {
            // Arrange
            Mini.Context context = new Mini.Context();
            CommandManager commandManager = new CommandManager(context);
            bool commandWasCalled = false;
            commandManager.AddCommandListener<TestExecutableCommand1>(
                delegate(TestExecutableCommand1 command)
                {
                    commandWasCalled = true;
                });
            
            // Act
            commandManager.InvokeCommand(new TestCommand2());  //Sending 2, not 1

            // Assert
            Assert.That(commandWasCalled, Is.False);
        }
        
        [Test]
        public void ExecutableCommandWasCalled_IsTrue_WhenExecuteExecutableCommand()
        {
            // Arrange
            Mini.Context context = new Mini.Context();
            CommandManager commandManager = new CommandManager(context);
            bool commandWasCalled = false;
            commandManager.AddCommandListener<TestExecutableCommand1>(
                delegate(TestExecutableCommand1 command)
                {
                    commandWasCalled = true;
                });
            
            // Act
            commandManager.ExecuteCommand(new TestExecutableCommand1());

            // Assert
            Assert.That(commandWasCalled, Is.True);
        }
        
        [Test]
        public void ExecutableCommandWasCalled_IsFalse_WhenNotExecuteExecutableCommand()
        {
            // Arrange
            Mini.Context context = new Mini.Context();
            CommandManager commandManager = new CommandManager(context);
            bool commandWasCalled = false;
            commandManager.AddCommandListener<TestExecutableCommand1>(
                delegate(TestExecutableCommand1 command)
                {
                    commandWasCalled = true;
                });
            
            // Act
            commandManager.ExecuteCommand(new TestExecutableCommand2()); //Sending 2, not 1

            // Assert
            Assert.That(commandWasCalled, Is.False);
        }
        
        [Test]
        public void Execute_IsCalled_WhenExecuteExecutableCommand()
        {
            // Arrange
            Mini.Context context = new Mini.Context();
            
            TestModel testModel = new TestModel();
            testModel.Counter.Value = 0;
            context.ModelLocator.AddItem(testModel);
            
            CommandManager commandManager = new CommandManager(context);
            
            // Act
            commandManager.ExecuteCommand(new IncrementCounterExecutableCommand()); 

            // Assert
            Assert.That(testModel.Counter.Value, Is.EqualTo(1));
        }
        
        
        [Test]
        public void Execute_IsNotCalled_WhenInvokeExecutableCommand()
        {
            // Arrange
            Mini.Context context = new Mini.Context();
            
            TestModel testModel = new TestModel();
            testModel.Counter.Value = 0;
            context.ModelLocator.AddItem(testModel);
            
            CommandManager commandManager = new CommandManager(context);
            
            // Act
            commandManager.InvokeCommand(new IncrementCounterExecutableCommand()); //invoke, not execute

            // Assert
            Assert.That(testModel.Counter.Value, Is.EqualTo(0));
        }
        
        
        [Test]
        public void Undo_IsCalled_WhenExecuteUndoExecutableCommand()
        {
            // Arrange
            Mini.Context context = new Mini.Context();
            
            TestModel testModel = new TestModel();
            testModel.Counter.Value = 0;
            context.ModelLocator.AddItem(testModel);
            
            CommandManager commandManager = new CommandManager(context);
            
            // Act
            commandManager.ExecuteCommand(new IncrementCounterExecutableCommand()); 
            commandManager.UndoCommand(new IncrementCounterExecutableCommand()); 

            // Assert
            Assert.That(testModel.Counter.Value, Is.EqualTo(0));
        }
        
        [Test]
        public void Execute_IsCalled_WhenExecuteCompoundExecutableCommand()
        {
            // Arrange
            Mini.Context context = new Mini.Context();
            
            TestModel testModel = new TestModel();
            testModel.Counter.Value = 0;
            context.ModelLocator.AddItem(testModel);
            
            CommandManager commandManager = new CommandManager(context);
            
            // Act
            commandManager.ExecuteCommand(new IncrementCounter3CompoundExecutableCommand()); 

            // Assert
            Assert.That(testModel.Counter.Value, Is.EqualTo(3));
        }
        
        [Test]
        public void Undo_IsCalled_WhenExecuteUndoCompoundExecutableCommand()
        {
            // Arrange
            Mini.Context context = new Mini.Context();
            
            TestModel testModel = new TestModel();
            testModel.Counter.Value = 0;
            context.ModelLocator.AddItem(testModel);
            
            CommandManager commandManager = new CommandManager(context);
            
            // Act
            commandManager.ExecuteCommand(new IncrementCounter3CompoundExecutableCommand()); 
            commandManager.UndoCommand(new IncrementCounter3CompoundExecutableCommand()); 

            // Assert
            Assert.That(testModel.Counter.Value, Is.EqualTo(0));
        }
        
        [Test]
        public void AddCommandListenerBothAreCalled_IsTrue_WhenExecuteUndoExecutableCommand()
        {
            // Arrange
            Mini.Context context = new Mini.Context();
            CommandManager commandManager = new CommandManager(context);
            bool executeWasCalled = false;
            bool undoWasCalled = false;
            commandManager.AddCommandListener<TestExecutableCommand1>(
                delegate(TestExecutableCommand1 command)
                {
                    //On Execute
                    executeWasCalled = true;
                },
                delegate(TestExecutableCommand1 command)
                {
                    //On undo
                    undoWasCalled = true;
                });
            
            // Act
            commandManager.ExecuteCommand(new TestExecutableCommand1());
            commandManager.UndoCommand(new TestExecutableCommand1());

            // Assert
            Assert.That(executeWasCalled, Is.True);
            Assert.That(undoWasCalled, Is.True);
        }
        
        [Test]
        public void AddCommandListenerUndoWasCalled_IsFalse_WhenExecutableCommandNotCanUndo()
        {
            // Arrange
            Mini.Context context = new Mini.Context();
            CommandManager commandManager = new CommandManager(context);
            bool executeWasCalled = false;
            bool undoWasCalled = false;
            commandManager.AddCommandListener<TestExecutableCommandWithoutUndo>(
                delegate(TestExecutableCommandWithoutUndo command)
                {
                    //On Execute
                    executeWasCalled = true;
                },
                delegate(TestExecutableCommandWithoutUndo command)
                {
                    //On undo
                    undoWasCalled = true;
                });
            
            // Act
            commandManager.ExecuteCommand(new TestExecutableCommandWithoutUndo());
            commandManager.UndoCommand(new TestExecutableCommandWithoutUndo());

            // Assert
            Assert.That(executeWasCalled, Is.True);
            Assert.That(undoWasCalled, Is.False);
        }
    }
}
