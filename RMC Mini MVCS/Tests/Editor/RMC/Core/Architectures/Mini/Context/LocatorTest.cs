using System;
using NUnit.Framework;


namespace RMC.Core.Architectures.Mini.Context
{
    /// <summary>
    /// In RMC Mini, we typically use the <see cref="Locator"/>
    /// in very specific subclasses of <see cref="ModelLocator"/>.
    ///
    /// Here we test a custom subclass of <see cref="Locator"/>.
    /// </summary>
    [Category ("RMC.Mini")]
    public class LocatorTest
    {
        public interface ISample
        {
            
        }

        public class SampleLocator : Locator<ISample>
        {
        }
        
        /// <summary>
        /// Main test
        /// </summary>
        public class Sample01 : ISample
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
        public class Sample02 : ISample
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
        public class Sample03 : Sample01
        {
        }
        
        
        [Test]
        public void AddItem_ThrowsException_WhenAddItemSameTwiceSameObject()
        {
            // Arrange
            SampleLocator sampleLocator = new SampleLocator();
            Sample01 sample01A = new Sample01();
            sampleLocator.AddItem(sample01A);
                       
            // Assert
            Assert.Throws<Exception>(() =>
            {
                // Act
                sampleLocator.AddItem(sample01A);
            });
        }
        
        [Test]
        public void AddItem_ThrowsNoException_WhenAddItemSameTwiceSameObjectWithName()
        {
            // Arrange
            SampleLocator sampleLocator = new SampleLocator();
            Sample01 sample01A = new Sample01();
            sampleLocator.AddItem(sample01A, "first");
                       
            // Assert
            Assert.DoesNotThrow(() =>
            {
                // Act
                sampleLocator.AddItem(sample01A, "second");
            });
        }
        
        [Test]
        public void AddItem_ThrowsNoException_WhenAddItemSameTwiceSameParentClass()
        {
            // Arrange
            SampleLocator sampleLocator = new SampleLocator();
            Sample01 sample01 = new Sample01();
            Sample02 sample02 = new Sample02();
            sampleLocator.AddItem(sample01);
  
            // Assert
            Assert.DoesNotThrow(() =>
            {
                // Act
                sampleLocator.AddItem(sample02);
            });
        }
        
        [Test]
        public void AddItem_ThrowsNoException_WhenAddItemSameTwiceChildClass()
        {
            // Arrange
            SampleLocator sampleLocator = new SampleLocator();
            Sample01 sample01 = new Sample01();
            Sample03 sample03 = new Sample03();
            sampleLocator.AddItem(sample01);
  
            // Assert
            Assert.DoesNotThrow(() =>
            {
                // Act
                sampleLocator.AddItem(sample03);
            });
        }
        
        [Test]
        public void HasItem_IsFalse_WhenNotAddItem()
        {
            // Arrange
            SampleLocator sampleLocator = new SampleLocator();
            Sample01 sample01 = new Sample01();
            
            // Act
            bool hasItem = sampleLocator.HasItem<Sample01>();

            // Assert
            Assert.That(hasItem, Is.False);
        }
        
        [Test]
        public void HasItem_IsFalse_WhenNotAddItemWithName()
        {
            // Arrange
            SampleLocator sampleLocator = new SampleLocator();
            Sample01 sample01 = new Sample01();
            
            // Act
            bool hasItem = sampleLocator.HasItem<Sample01>("01");

            // Assert
            Assert.That(hasItem, Is.False);
        }
        
        [Test]
        public void HasItem_IsTrue_WhenAddItem()
        {
            // Arrange
            SampleLocator sampleLocator = new SampleLocator();
            sampleLocator.AddItem(new Sample01());
            
            // Act
            bool hasItem = sampleLocator.HasItem<Sample01>();

            // Assert
            Assert.That(hasItem, Is.True);
        }
        
        [Test]
        public void HasItem_IsTrue_WhenAddItemWithName()
        {
            // Arrange
            SampleLocator sampleLocator = new SampleLocator();
            sampleLocator.AddItem(new Sample01(), "01");
            
            // Act
            bool hasItem = sampleLocator.HasItem<Sample01>("01");

            // Assert
            Assert.That(hasItem, Is.True);
        }
        
        [Test]
        public void HasItem_ResultIsFalse_WhenAddItemSameObjectWithWrongName()
        {
            // Arrange
            SampleLocator sampleLocator = new SampleLocator();
            Sample01 sample01A = new Sample01();
            sampleLocator.AddItem(sample01A, "01");
  
            // Act
            bool hasItem = sampleLocator.HasItem<Sample01>("02");

            // Assert
            Assert.That(hasItem, Is.False);
        }
        
        [Test]
        public void GetItem01_ResultIs01_WhenGetItem01_Sibling()
        {
            // Arrange
            SampleLocator sampleLocator = new SampleLocator();
            Sample01 sample01 = new Sample01();
            Sample02 sample02 = new Sample02();
            sampleLocator.AddItem(sample01);
            sampleLocator.AddItem(sample02);
            
            // Act
            Sample01 found = sampleLocator.GetItem<Sample01>();

            // Assert
            Assert.That(found, Is.SameAs(sample01));
        }
        
        [Test]
        public void GetItem01_ResultIs01_WhenGetItem01_SiblingWithName()
        {
            // Arrange
            SampleLocator sampleLocator = new SampleLocator();
            Sample01 sample01 = new Sample01();
            Sample02 sample02 = new Sample02();
            sampleLocator.AddItem(sample01, "01");
            sampleLocator.AddItem(sample02, "02");
            
            // Act
            Sample01 found = sampleLocator.GetItem<Sample01>("01");

            // Assert
            Assert.That(found, Is.SameAs(sample01));
        }
        
        [Test]
        public void GetItem02_ResultIs02_WhenGetItem02_Sibling()
        {
            // Arrange
            SampleLocator sampleLocator = new SampleLocator();
            Sample01 sample01 = new Sample01();
            Sample02 sample02 = new Sample02();
            sampleLocator.AddItem(sample01);
            sampleLocator.AddItem(sample02);
            
            // Act
            Sample02 found = sampleLocator.GetItem<Sample02>();

            // Assert
            Assert.That(found, Is.SameAs(sample02));
        }
        
        [Test]
        public void GetItem02_ResultIs02_WhenGetItem02_SiblingWithName()
        {
            // Arrange
            SampleLocator sampleLocator = new SampleLocator();
            Sample01 sample01 = new Sample01();
            Sample02 sample02 = new Sample02();
            sampleLocator.AddItem(sample01, "01");
            sampleLocator.AddItem(sample02, "02");
            
            // Act
            Sample02 found = sampleLocator.GetItem<Sample02>("02");

            // Assert
            Assert.That(found, Is.SameAs(sample02));
        }
        
        [Test]
        public void GetItem01_ResultIs01_WhenGetItem01_Child()
        {
            // Arrange
            SampleLocator sampleLocator = new SampleLocator();
            Sample01 sample01 = new Sample01();
            Sample03 sample03 = new Sample03();
            sampleLocator.AddItem(sample01);
            sampleLocator.AddItem(sample03);
 
            // Act
            Sample01 found = sampleLocator.GetItem<Sample01>();

            // Assert
            Assert.That(found, Is.SameAs(sample01));
        }
        
        [Test]
        public void GetItem01_ResultIs01_WhenGetItem01_ChildWithName()
        {
            // Arrange
            SampleLocator sampleLocator = new SampleLocator();
            Sample01 sample01 = new Sample01();
            Sample03 sample03 = new Sample03();
            sampleLocator.AddItem(sample01, "01");
            sampleLocator.AddItem(sample03, "03");
 
            // Act
            Sample01 found = sampleLocator.GetItem<Sample01>("01");

            // Assert
            Assert.That(found, Is.SameAs(sample01));
        }
        
        [Test]
        public void GetItem03_ResultIs03_WhenGetItem03_Child()
        {
            // Arrange
            SampleLocator sampleLocator = new SampleLocator();
            Sample01 sample01 = new Sample01();
            Sample03 sample03 = new Sample03();
            sampleLocator.AddItem(sample01);
            sampleLocator.AddItem(sample03);
            
            // Act
            Sample03 found = sampleLocator.GetItem<Sample03>();

            // Assert
            Assert.That(found, Is.SameAs(sample03));
        }
        
        
        [Test]
        public void GetItem_ResultIsNull_WhenAddItemSameObjectWithWrongName()
        {
            // Arrange
            SampleLocator sampleLocator = new SampleLocator();
            Sample01 sample01A = new Sample01();
            sampleLocator.AddItem(sample01A, "01");
  
            // Act
            Sample01 found = sampleLocator.GetItem<Sample01>("02");

            // Assert
            Assert.That(found, Is.Null);
        }
        
        [Test]
        public void GetItem_ResultIsNotNull_WhenAddItemSameClassIsStrict()
        {
            // Arrange
            SampleLocator sampleLocator = new SampleLocator();
            Sample01 sample01 = new Sample01();
            sampleLocator.AddItem(sample01);
  
            // Act
            Sample01 found = sampleLocator.GetItem<Sample01>();

            // Assert
            Assert.That(found, Is.Not.Null);
        }
        
        [Test]
        public void GetItem01_Result03IsNotNull_WhenAddItemSameParentClassIsNotStrict()
        {
            // Arrange
            SampleLocator sampleLocator = new SampleLocator();
            Sample01 sample01 = new Sample03();
            sampleLocator.AddItem(sample01);
  
            // Act
            Sample01 found = sampleLocator.GetItem<Sample01>();

            // Assert
            Assert.That(found, Is.Not.Null);
        }
        
        [Test]
        public void GetItem03_Result01IsNull_WhenAddItemSameParentClassIsNotStrict()
        {
            // Arrange
            SampleLocator sampleLocator = new SampleLocator();
            Sample01 sample01 = new Sample01();
            sampleLocator.AddItem(sample01);
  
            // Act
            Sample01 found = sampleLocator.GetItem<Sample03>();

            // Assert
            Assert.That(found, Is.Null);
        }
        
        
        [Test]
        public void GetItem03_Result01IsNull_WhenAddItemSameParentClassIsStrict()
        {
            // Arrange
            SampleLocator sampleLocator = new SampleLocator();
            Sample01 sample01 = new Sample01();
            sampleLocator.AddItem(sample01);
  
            // Act
            Sample01 found = sampleLocator.GetItem<Sample03>();

            // Assert
            Assert.That(found, Is.Null);
        }
        
        [Test]
        public void HasItem_IsFalse_WhenAddRemoveItem()
        {
            // Arrange
            SampleLocator sampleLocator = new SampleLocator();
            Sample01 sample01 = new Sample01();
            sampleLocator.AddItem(sample01);
            sampleLocator.RemoveItem<Sample01>();
            
            // Act
            bool hasItem = sampleLocator.HasItem<Sample01>();

            // Assert
            Assert.That(hasItem, Is.False);
        }
        
        [Test]
        public void RemoveItem_ThrowsError_WhenItemNotExisting()
        {
            // Arrange
            SampleLocator sampleLocator = new SampleLocator();
            Sample01 sample01 = new Sample01();
       
            // Assert
            Assert.Throws<Exception>(() =>
            {
                // Act
                sampleLocator.RemoveItem<Sample01>();
            });
        }

        [Test]
        public void RemoveItemInstance_ThrowsError_WhenItemNotExisting()
        {
            // Arrange
            SampleLocator sampleLocator = new SampleLocator();

            // Assert
            Assert.Throws<Exception>(() =>
            {
                // Act
                sampleLocator.RemoveItem<Sample01>();
            });
        }

    }
}
