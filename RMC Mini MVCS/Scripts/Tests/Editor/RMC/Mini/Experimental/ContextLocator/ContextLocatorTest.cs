using NUnit.Framework;
using RMC.Mini.Controller.Commands;
using RMC.Mini.Model;

namespace RMC.Mini.Experimental.ContextLocators
{
    public class MockContext : IContext
    {
        // Implement the IContext interface for testing purposes
        public void Dispose()
        {
            // TODO release managed resources here
        }

        public Locator<IModel> ModelLocator { get; }
        public ICommandManager CommandManager { get; }
    }

    [TestFixture]
    public class ContextLocatorTests
    {
        [SetUp]
        public void SetUp()
        {
            global::RMC.Mini.Experimental.ContextLocators.ContextLocator.Destroy();
        }

        [Test]
        public void Instance_ShouldReturnSingletonInstance()
        {
            var instance1 = global::RMC.Mini.Experimental.ContextLocators.ContextLocator.Instance;
            var instance2 = global::RMC.Mini.Experimental.ContextLocators.ContextLocator.Instance;

            Assert.AreSame(instance1, instance2);
        }

        [Test]
        public void Destroy_ShouldClearInstance()
        {
            var instance1 = global::RMC.Mini.Experimental.ContextLocators.ContextLocator.Instance;
            global::RMC.Mini.Experimental.ContextLocators.ContextLocator.Destroy();
            var instance2 = global::RMC.Mini.Experimental.ContextLocators.ContextLocator.Instance;

            Assert.AreNotSame(instance1, instance2);
        }

        [Test]
        public void AddAndGetItem_ShouldStoreAndRetrieveContext()
        {
            var context = new MockContext();
            var locator = global::RMC.Mini.Experimental.ContextLocators.ContextLocator.Instance;
            locator.AddItem(context);

            var retrievedContext = locator.GetItem<MockContext>();

            Assert.AreSame(context, retrievedContext);
        }

        [TearDown]
        public void TearDown()
        {
            global::RMC.Mini.Experimental.ContextLocators.ContextLocator.Destroy();
        }
    }
}