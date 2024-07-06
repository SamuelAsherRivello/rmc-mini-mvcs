using System;
using NUnit.Framework;

namespace RMC.Mini.Locators
{
   
    [Category("RMC.Mini.Locators")]
    public class NestedLocatorTest
    {
        
        public interface IA : IDisposable
        {
        }

        public class SampleA : IA
        {
            public void Dispose()
            {
                // TODO release managed resources here
            }
        }

        public class SampleSubA : SampleA
        {
        }
    
        public class SampleSubASubB: SampleSubA
        {
        }
    
        public class GenericClass<T> : IA
        {
            public Locator<T> Locator { get; set; }
        
            public GenericClass()
            {
                Locator = new Locator<T>();
            }

            public void Dispose()
            {
                // Do any cleanup
            }
        }

        
        [Test]
        public void AddItem_ShouldAddItemToGenericLocator()
        {
            var genericClass = new GenericClass<SampleA>();
            var item = new SampleA();
            genericClass.Locator.AddItem(item);
            Assert.IsTrue(genericClass.Locator.HasItem<SampleA>());
        }

        [Test]
        public void GetItem_ShouldReturnItemFromGenericLocator()
        {
            var genericClass = new GenericClass<SampleA>();
            var item = new SampleA();
            genericClass.Locator.AddItem(item);
            var retrievedItem = genericClass.Locator.GetItem<SampleA>();
            Assert.AreSame(item, retrievedItem);
        }

        [Test]
        public void HasItem_ShouldReturnTrueForSubtype()
        {
            var genericClass = new GenericClass<SampleA>();
            var item = new SampleSubA();
            genericClass.Locator.AddItem(item);
            Assert.IsTrue(genericClass.Locator.HasItem<SampleSubA>());
        }

        [Test]
        public void GetItem_ShouldReturnNullForBaseTypeWhenSubtypeAdded()
        {
            var genericClass = new GenericClass<SampleA>();
            var item = new SampleSubA();
            genericClass.Locator.AddItem(item);
            var retrievedItem = genericClass.Locator.GetItem<SampleA>();
            Assert.IsNull(retrievedItem);
        }

        [Test]
        public void GetItem_ShouldReturnItemForExactSubtype()
        {
            var genericClass = new GenericClass<SampleA>();
            var item = new SampleSubASubB();
            genericClass.Locator.AddItem(item);
            var retrievedItem = genericClass.Locator.GetItem<SampleSubASubB>();
            Assert.AreSame(item, retrievedItem);
        }

        [Test]
        public void AddItem_ShouldThrowExceptionForDuplicateWithoutKey()
        {
            var genericClass = new GenericClass<SampleA>();
            var item1 = new SampleA();
            var item2 = new SampleA();
            genericClass.Locator.AddItem(item1);
            Assert.Throws<Exception>(() => genericClass.Locator.AddItem(item2));
        }

        [Test]
        public void AddItem_WithDifferentKeys_ShouldAddItems()
        {
            var genericClass = new GenericClass<SampleA>();
            var item1 = new SampleA();
            var item2 = new SampleA();
            genericClass.Locator.AddItem(item1, "key1");
            genericClass.Locator.AddItem(item2, "key2");
            Assert.IsTrue(genericClass.Locator.HasItem<SampleA>("key1"));
            Assert.IsTrue(genericClass.Locator.HasItem<SampleA>("key2"));
        }

        [Test]
        public void RemoveItem_ShouldRemoveItemFromGenericLocator()
        {
            var genericClass = new GenericClass<SampleA>();
            var item = new SampleA();
            genericClass.Locator.AddItem(item);
            genericClass.Locator.RemoveAndDisposeItem<SampleA>();
            Assert.IsFalse(genericClass.Locator.HasItem<SampleA>());
        }
    }
}
