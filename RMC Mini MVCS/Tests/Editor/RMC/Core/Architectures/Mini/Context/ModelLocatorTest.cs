using System;
using NUnit.Framework;
using RMC.Core.Architectures.Mini.Model;
using UnityEngine;

namespace RMC.Core.Architectures.Mini.Context
{
    [Category ("RMC.Mini")]
    public class ModelLocatorTest
    {
        
        /// <summary>
        /// Main test
        /// </summary>
        public class TestModel01 : IModel
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
        
        /// <summary>
        /// sibling test
        /// </summary>
        public class TestModel02 : IModel
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
        
        /// <summary>
        /// Child test
        /// </summary>
        public class TestModel03 : TestModel01
        {
        }
        
        
        [Test]
        public void HasItem_IsTrue_WhenAddItem()
        {
            // Arrange
            ModelLocator modelLocator = new ModelLocator();
            TestModel01 testModel01 = new TestModel01();
            
            // Act
            bool hasItem = modelLocator.HasItem<TestModel01>();

            // Assert
            Assert.That(hasItem, Is.False);
        }
        
        [Test]
        public void HasItem_IsFalse_WhenNoAddItem()
        {
            // Arrange
            ModelLocator modelLocator = new ModelLocator();
            
            // Act
            bool hasItem = modelLocator.HasItem<TestModel01>();

            // Assert
            Assert.That(hasItem, Is.False);
        }
        
        [Test]
        public void GetItem01_ResultIs01_WhenGetItem01_Sibling()
        {
            // Arrange
            ModelLocator modelLocator = new ModelLocator();
            TestModel01 testModel01 = new TestModel01();
            TestModel02 testModel02 = new TestModel02();
            modelLocator.AddItem(testModel01);
            modelLocator.AddItem(testModel02);
            
            // Act
            TestModel01 found = modelLocator.GetItem<TestModel01>();

            // Assert
            Assert.That(found, Is.SameAs(testModel01));
        }
        
        [Test]
        public void GetItem02_ResultIs02_WhenGetItem02_Sibling()
        {
            // Arrange
            ModelLocator modelLocator = new ModelLocator();
            TestModel01 testModel01 = new TestModel01();
            TestModel02 testModel02 = new TestModel02();
            modelLocator.AddItem(testModel01);
            modelLocator.AddItem(testModel02);
            
            // Act
            TestModel02 found = modelLocator.GetItem<TestModel02>();

            // Assert
            Assert.That(found, Is.SameAs(testModel02));
        }
        
        [Test]
        public void GetItem01_ResultIs01_WhenGetItem01_Child()
        {
            // Arrange
            ModelLocator modelLocator = new ModelLocator();
            TestModel01 testModel01 = new TestModel01();
            TestModel03 testModel03 = new TestModel03();
            modelLocator.AddItem(testModel01);
            modelLocator.AddItem(testModel03);
 
            
            // Act
            TestModel01 found = modelLocator.GetItem<TestModel01>();

            // Assert
            Assert.That(found, Is.SameAs(testModel01));
        }
        
        [Test]
        public void GetItem03_ResultIs03_WhenGetItem03_Child()
        {
            // Arrange
            ModelLocator modelLocator = new ModelLocator();
            TestModel01 testModel01 = new TestModel01();
            TestModel03 testModel03 = new TestModel03();
            modelLocator.AddItem(testModel01);
            modelLocator.AddItem(testModel03);
            
            // Act
            TestModel03 found = modelLocator.GetItem<TestModel03>();

            // Assert
            Assert.That(found, Is.SameAs(testModel03));
        }
        
        
        [Test]
        public void AddItem_ThrowsException_WhenAddItemSameTwiceSameObject()
        {
            // Arrange
            ModelLocator modelLocator = new ModelLocator();
            TestModel01 testModel01a = new TestModel01();
            modelLocator.AddItem(testModel01a);
                       
            // Assert
            Assert.Throws<Exception>(() =>
            {
                // Act
                modelLocator.AddItem(testModel01a);
            }, "I want this to throw an exception. It does not properly do that yet.");
        }


        [Test]
        public void AddItem_ThrowsNoException_WhenAddItemSameTwiceSameClass()
        {
            // Arrange
            ModelLocator modelLocator = new ModelLocator();
            TestModel01 testModel01a = new TestModel01();
            TestModel01 testModel01b = new TestModel01();
            modelLocator.AddItem(testModel01a);
  
            // Assert
            Assert.DoesNotThrow(() =>
            {
                // Act
                modelLocator.AddItem(testModel01b);
            });
        }
        
        [Test]
        public void AddItem_ThrowsNoException_WhenAddItemSameTwiceSameParentClass()
        {
            // Arrange
            ModelLocator modelLocator = new ModelLocator();
            TestModel01 testModel01 = new TestModel01();
            TestModel02 testModel02 = new TestModel02();
            modelLocator.AddItem(testModel01);
  
            // Assert
            Assert.DoesNotThrow(() =>
            {
                // Act
                modelLocator.AddItem(testModel02);
            });
        }
        
        [Test]
        public void AddItem_ThrowsNoException_WhenAddItemSameTwiceChildClass()
        {
            // Arrange
            ModelLocator modelLocator = new ModelLocator();
            TestModel01 testModel01 = new TestModel01();
            TestModel03 testModel03 = new TestModel03();
            modelLocator.AddItem(testModel01);
  
            // Assert
            Assert.DoesNotThrow(() =>
            {
                // Act
                modelLocator.AddItem(testModel03);
            });
        }
        
        [Test]
        public void GetItem_ResultIsNotNull_WhenAddItemSameObject()
        {
            // Arrange
            ModelLocator modelLocator = new ModelLocator();
            TestModel01 testModel01a = new TestModel01();
            modelLocator.AddItem(testModel01a);
  
            // Act
            TestModel01 found = modelLocator.GetItem<TestModel01>();

            // Assert
            Assert.That(found, Is.Not.Null);
        }
        
        [Test]
        public void GetItem_ResultIsNotNull_WhenAddItemSameClassNotStrict()
        {
            // Arrange
            const bool isStrict = false;
            ModelLocator modelLocator = new ModelLocator();
            TestModel01 testModel01 = new TestModel01();
            modelLocator.AddItem(testModel01);
  
            // Act
            TestModel01 found = modelLocator.GetItem<TestModel01>(isStrict);

            // Assert
            Assert.That(found, Is.Not.Null);
        }
        
        [Test]
        public void GetItem_ResultIsNotNull_WhenAddItemSameClassIsStrict()
        {
            // Arrange
            const bool isStrict = true;
            ModelLocator modelLocator = new ModelLocator();
            TestModel01 testModel01 = new TestModel01();
            modelLocator.AddItem(testModel01);
  
            // Act
            TestModel01 found = modelLocator.GetItem<TestModel01>(isStrict);

            // Assert
            Assert.That(found, Is.Not.Null);
        }
        
        [Test]
        public void GetItem01_Result03IsNotNull_WhenAddItemSameParentClassIsNotStrict()
        {
            // Arrange
            const bool isStrict = false;
            ModelLocator modelLocator = new ModelLocator();
            TestModel01 testModel03 = new TestModel03();
            modelLocator.AddItem(testModel03);
  
            // Act
            TestModel01 found = modelLocator.GetItem<TestModel01>(isStrict);

            // Assert
            Assert.That(found, Is.Not.Null);
        }
        
        [Test]
        public void GetItem03_Result01IsNull_WhenAddItemSameParentClassIsNotStrict()
        {
            // Arrange
            const bool isStrict = false;
            ModelLocator modelLocator = new ModelLocator();
            TestModel01 testModel01 = new TestModel01();
            modelLocator.AddItem(testModel01);
  
            // Act
            TestModel01 found = modelLocator.GetItem<TestModel03>(isStrict);

            // Assert
            Assert.That(found, Is.Null);
        }
        
        
        [Test]
        public void GetItem03_Result01IsNull_WhenAddItemSameParentClassIsStrict()
        {
            // Arrange
            const bool isStrict = true;
            ModelLocator modelLocator = new ModelLocator();
            TestModel01 testModel01 = new TestModel01();
            modelLocator.AddItem(testModel01);
  
            // Act
            TestModel01 found = modelLocator.GetItem<TestModel03>(isStrict);

            // Assert
            Assert.That(found, Is.Null);
        }
        
        [Test]
        public void HasItem_IsFalse_WhenAddRemoveItem()
        {
            // Arrange
            ModelLocator modelLocator = new ModelLocator();
            TestModel01 testModel01 = new TestModel01();
            modelLocator.AddItem(testModel01);
            modelLocator.RemoveItem<TestModel01>();
            
            // Act
            bool hasItem = modelLocator.HasItem<TestModel01>();

            // Assert
            Assert.That(hasItem, Is.False);
        }
        
        [Test]
        public void RemoveItem_ThrowsError_WhenItemNotExisting()
        {
            // Arrange
            ModelLocator modelLocator = new ModelLocator();
            TestModel01 testModel01 = new TestModel01();
       
            // Assert
            Assert.Throws<Exception>(() =>
            {
                // Act
                modelLocator.RemoveItem<TestModel01>();
            });
        }

        [Test]
        public void RemoveItemInstance_ThrowsError_WhenItemNotExisting()
        {
            // Arrange
            ModelLocator modelLocator = new ModelLocator();
            TestModel01 testModel01 = new TestModel01();

            // Assert
            Assert.Throws<Exception>(() =>
            {
                // Act
                modelLocator.RemoveItem<TestModel01>(testModel01);
            });
        }

    }
}
