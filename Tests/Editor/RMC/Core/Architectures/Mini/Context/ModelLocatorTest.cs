using System;
using NUnit.Framework;
using RMC.Core.Architectures.Mini.Model;

namespace RMC.Core.Architectures.Mini.Context
{
    [Category ("RMC.Mini")]
    public class ModelLocatorTest
    {
        public class TestModel : IModel
        {
            public bool IsInitialized { get; }
            public IContext Context { get; }
            public void Initialize(IContext context)
            {
                throw new System.NotImplementedException();
            }

            public void RequireIsInitialized()
            {
                throw new System.NotImplementedException();
            }
        }
        
        public class TestModel2 : IModel
        {
            public bool IsInitialized { get; }
            public IContext Context { get; }
            public void Initialize(IContext context)
            {
                throw new System.NotImplementedException();
            }

            public void RequireIsInitialized()
            {
                throw new System.NotImplementedException();
            }
        }
        
        [Test]
        public void HasItem_IsTrue_WhenAddItem()
        {
            // Arrange
            ModelLocator modelLocator = new ModelLocator();
            TestModel testModel = new TestModel();
            
            // Act
            bool hasItem = modelLocator.HasItem<TestModel>();

            // Assert
            Assert.That(hasItem, Is.False);
        }
        
        [Test]
        public void HasItem_IsFalse_WhenNoAddItem()
        {
            // Arrange
            ModelLocator modelLocator = new ModelLocator();
            
            // Act
            bool hasItem = modelLocator.HasItem<TestModel>();

            // Assert
            Assert.That(hasItem, Is.False);
        }
        
        [Test]
        public void GetItem_ResultIsSame_WhenGetItem()
        {
            // Arrange
            ModelLocator modelLocator = new ModelLocator();
            TestModel testModel = new TestModel();
            TestModel2 testModel2 = new TestModel2();
            modelLocator.AddItem(testModel);
            
            // Act
            TestModel found = modelLocator.GetItem<TestModel>();

            // Assert
            Assert.That(found, Is.SameAs(testModel));
        }
        
                
        [Test]
        public void GetItem_ThrowsException_WhenAddItemSameTwice()
        {
            // Arrange
            ModelLocator modelLocator = new ModelLocator();
            TestModel testModel = new TestModel();
            modelLocator.AddItem(testModel);
  
            // Assert
            Assert.Throws<Exception>(() =>
            {
                // Act
                modelLocator.AddItem(testModel);
            });
        }
    }
}
