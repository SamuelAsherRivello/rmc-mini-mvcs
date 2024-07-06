using NUnit.Framework;

namespace RMC.Mini.Contexts
{
    [Category ("RMC.Mini")]
    public class ContextTest
    {
        private BaseContext _baseContext;

        [SetUp]
        public void SetUp()
        {
            _baseContext = new BaseContext();
        }

        [Test]
        public void CommandManager_ShouldNotBeNullAfterInitialization()
        {
            Assert.IsNotNull(_baseContext.CommandManager);
        }

        [Test]
        public void ModelLocator_ShouldNotBeNullAfterInitialization()
        {
            Assert.IsNotNull(_baseContext.ModelLocator);
        }

        [Test]
        public void CommandManager_NotNullAndEmpty_AfterInitialization()
        {
            _baseContext.Dispose();
            Assert.IsNotNull(_baseContext.CommandManager.GetCommandListenerCount());
        }

        [Test]
        public void ModelLocator_NotNullAndEmpty_AfterInitialization()
        {
            _baseContext.Dispose();
            Assert.AreEqual(_baseContext.ModelLocator.GetItemCount(), 0);
        }
        
    }
}