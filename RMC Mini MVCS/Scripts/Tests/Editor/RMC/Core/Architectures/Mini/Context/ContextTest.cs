using System;
using NUnit.Framework;
using RMC.Core.Experimental;
using UnityEngine;

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
        public void Context_ThrowsException_WhenCreatedTwice_WithoutUniqueContextKey()
        {
            // Arrange
            
            // Assert

            Assert.Throws<Exception>(() =>
            {
                // Act
                Context context1 = new Context(); //has singleton inside
                Context context2 = new Context();
            });
        }
        
        [Test]
        public void Context_ThrowsNoException_WhenCreatedTwice_WithUniqueContextKey()
        {
            // Arrange
            
            // Act
            Assert.DoesNotThrow(() =>
            {
                Context context1 = new Context(); //has singleton inside
                Context context2 = new Context("uniqueKey");
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
            Context context = new Context();

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
            bool hasItem = ContextLocator.Instance.HasItem<Context>();

            // Assert
            Assert.That(hasItem, Is.False);
        }
        
        [Test]
        public void HasItem_IsTrue_WhenAdded()
        {
            // Arrange
            Context context = new Context(); //calls contextLocator.AddItem
            
            // Act
            Debug.Log("-------------1");
            bool hasItem = ContextLocator.Instance.HasItem<Context>();
            Debug.Log("-------------2");
            
            // Assert
            Assert.That(hasItem, Is.True);
        }
    }
}
