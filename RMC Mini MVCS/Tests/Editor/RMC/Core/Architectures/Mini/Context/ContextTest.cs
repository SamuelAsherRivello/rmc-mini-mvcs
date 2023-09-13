using NUnit.Framework;
using RMC.Core.Experimental;

namespace RMC.Core.Architectures.Mini.Context
{
    [Category ("RMC.Mini")]
    public class ContextTest
    {

        [TearDown]
        public void TearDown()
        {
            ContextLocator.Destroy();
        }
        
        [Test]
        public void Context_IsNotNull_WhenCreated()
        {
            // Arrange
            
            // Act
            Context context = new Context();

            // Assert
            Assert.That(context, Is.Not.Null);
        }
        
        
        [Test]
        public void Context_ThrowsNoExceptions_WhenCreatedTwice()
        {
            // Arrange
            
            // Act
            Assert.DoesNotThrow(() =>
            {
                Context context1 = new Context(); //has singleton inside
                Context context2 = new Context();
            });

            // Assert
        }
        
        [Test]
        public void OnAddItemCompleted_IsCalled_WhenAddItem()
        {
            // Arrange
            bool isCalled = false;
            ContextLocator.Instance.OnAddItemCompleted.AddListener(addedContext =>
            {
                isCalled = true;
            });
    
            // Act
            Context context = new Context();

            // Assert
            Assert.That(isCalled, Is.True);
        }
        
        [Test]
        public void OnAddItemCompleted_IsNotCalled_WhenNotAddItem()
        {
            // Arrange
            bool isCalled = false;
            ContextLocator.Instance.OnAddItemCompleted.AddListener(addedContext =>
            {
                isCalled = true;
            });
    
            // Act
            //(Do nothing)

            // Assert
            Assert.That(isCalled, Is.False);
        }
        
        [Test]
        public void HasItem_IsFalse_WhenNotAdded()
        {
            // Arrange
            
            // Act
            bool hasItem = ContextLocator.Instance.HasItem<IContext>();

            // Assert
            Assert.That(hasItem, Is.False);
        }
        
        [Test]
        public void HasItem_IsTrue_WhenAdded()
        {
            // Arrange
            Context context = new Context(); //calls addItem
            
            // Act
            bool hasItem = ContextLocator.Instance.HasItem<IContext>();

            // Assert
            Assert.That(hasItem, Is.True);
        }
    }
}
