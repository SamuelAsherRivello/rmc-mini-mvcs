using System;
using NUnit.Framework;

//Keep As:RMC.Mini
namespace RMC.Mini
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
            global::RMC.Mini.Context context = new global::RMC.Mini.Context();

            // Assert
            Assert.That(context, Is.Not.Null);
        }
        
        
        [Test]
        public void Context_ThrowsException_WhenCreatedTwice_WithoutUniqueContextKey()
        {
            // Arrange
            
            // Assert

            Assert.Throws<Exception>(() =>
            {
                // Act
                global::RMC.Mini.Context context1 = new global::RMC.Mini.Context(); //has singleton inside
                global::RMC.Mini.Context context2 = new global::RMC.Mini.Context();
                global::RMC.Mini.Context context3 = new global::RMC.Mini.Context();
            });
        }
        
        [Test]
        public void Context_ThrowsNoException_WhenCreatedTwice_WithUniqueContextKey()
        {
            // Arrange
            
            // Act
            Assert.DoesNotThrow(() =>
            {
                global::RMC.Mini.Context context1 = new global::RMC.Mini.Context(); //has singleton inside
                global::RMC.Mini.Context context2 = new global::RMC.Mini.Context("uniqueKey");
            });

            // Assert
        }
        
        [Test]
        public void OnAddItemCompleted_IsCalled_WhenAddItem()
        {
            // Arrange
            bool isCalled = false;
            ContextLocator.Instance.OnItemAdded.AddListener(addedContext =>
            {
                isCalled = true;
            });
    
            // Act
            global::RMC.Mini.Context context = new global::RMC.Mini.Context();

            // Assert
            Assert.That(isCalled, Is.True);
        }
        
        [Test]
        public void OnAddItemCompleted_IsNotCalled_WhenNotAddItem()
        {
            // Arrange
            bool isCalled = false;
            ContextLocator.Instance.OnItemAdded.AddListener(addedContext =>
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
            bool hasItem = ContextLocator.Instance.HasItem<global::RMC.Mini.Context>();

            // Assert
            Assert.That(hasItem, Is.False);
        }
        
        [Test]
        public void HasItem_IsTrue_WhenAdded()
        {
            // Arrange
            global::RMC.Mini.Context context = new global::RMC.Mini.Context(); //calls contextLocator.AddItem
            
            // Act
            bool hasItem = ContextLocator.Instance.HasItem<global::RMC.Mini.Context>();
            
            // Assert
            Assert.That(hasItem, Is.True);
        }
    }
}
