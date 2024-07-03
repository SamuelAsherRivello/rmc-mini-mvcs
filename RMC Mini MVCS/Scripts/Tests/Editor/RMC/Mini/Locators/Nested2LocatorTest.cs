using NUnit.Framework;
using UnityEngine;
using System;

namespace RMC.Mini.Locators
{
    public interface IA
    {
    }

    public class SampleA : IA
    {
    }

    public class SampleSubA : SampleA
    {
    }

    public class SampleSubASubB : SampleSubA
    {
    }

    public interface IContext
    {
        Locator<IA> ModelLocator { get; }
    }

    public class SampleContext : IContext
    {
        public Locator<IA> ModelLocator { get; private set; }

        public SampleContext()
        {
            ModelLocator = new Locator<IA>();
            Debug.Log($"SampleContext created with ModelLocator of type {ModelLocator.GetType().FullName}");
        }
    }

    public class GenericClass<T> where T : class, IA
    {
        private IContext _context;

        public Locator<T> ModelLocator => GetOrCreateLocator();

        public GenericClass(IContext context)
        {
            _context = context;
        }

        private Locator<T> GetOrCreateLocator()
        {
            var baseLocator = _context.ModelLocator;

            // Log the type check for debugging
            if (_context.ModelLocator is Locator<IA> locatorIA && locatorIA is Locator<T> locatorT)
            {
                return locatorT;
            }

            Debug.LogWarning($"Cannot cast ModelLocator to Locator<{typeof(T).FullName}>");
            return null;
        }
    }

    [Category("RMC.Mini.Locators")]
    public class Nested2LocatorTest
    {
        [Test]
        public void ModelLocator_ShouldReturnCorrectLocator()
        {
            var context = new SampleContext();
            var genericClass = new GenericClass<SampleA>(context);

            var modelLocator = genericClass.ModelLocator;
            Assert.IsNotNull(modelLocator, "ModelLocator should not be null");
        }

        [Test]
        public void AddItem_ShouldAddItemToModelLocator()
        {
            var context = new SampleContext();
            var genericClass = new GenericClass<SampleA>(context);

            var item = new SampleA();
            var modelLocator = genericClass.ModelLocator;
            Assert.IsNotNull(modelLocator, "ModelLocator should not be null");

            modelLocator.AddItem(item);

            Assert.IsTrue(modelLocator.HasItem<SampleA>(), "ModelLocator should have the item");
        }

        [Test]
        public void GetItem_ShouldReturnItemFromModelLocator()
        {
            var context = new SampleContext();
            var genericClass = new GenericClass<SampleA>(context);

            var item = new SampleA();
            var modelLocator = genericClass.ModelLocator;
            Assert.IsNotNull(modelLocator, "ModelLocator should not be null");

            modelLocator.AddItem(item);

            var retrievedItem = modelLocator.GetItem<SampleA>();
            Assert.AreSame(item, retrievedItem, "Retrieved item should be the same as the added item");
        }

        [Test]
        public void ModelLocator_ShouldReturnNullForInvalidCast()
        {
            var context = new SampleContext();
            var genericClass = new GenericClass<SampleSubA>(context);

            var modelLocator = genericClass.ModelLocator;

            if (modelLocator == null)
            {
                Debug.Log("ModelLocator is null as expected for invalid cast.");
            }
            else
            {
                Debug.LogError("ModelLocator should be null for invalid cast, but it is not.");
            }

            Assert.IsNull(modelLocator, "ModelLocator should be null for invalid cast");
        }

        [Test]
        public void AddItem_ShouldThrowExceptionForDuplicateWithoutKey()
        {
            var context = new SampleContext();
            var genericClass = new GenericClass<SampleA>(context);

            var item1 = new SampleA();
            var item2 = new SampleA();
            var modelLocator = genericClass.ModelLocator;
            Assert.IsNotNull(modelLocator, "ModelLocator should not be null");

            modelLocator.AddItem(item1);
            Assert.Throws<Exception>(() => modelLocator.AddItem(item2), "Adding a duplicate item without a key should throw an exception");
        }

        [Test]
        public void AddItem_WithDifferentKeys_ShouldAddItems()
        {
            var context = new SampleContext();
            var genericClass = new GenericClass<SampleA>(context);

            var item1 = new SampleA();
            var item2 = new SampleA();
            var modelLocator = genericClass.ModelLocator;
            Assert.IsNotNull(modelLocator, "ModelLocator should not be null");

            modelLocator.AddItem(item1, "key1");
            modelLocator.AddItem(item2, "key2");

            Assert.IsTrue(modelLocator.HasItem<SampleA>("key1"), "ModelLocator should have the item with key 'key1'");
            Assert.IsTrue(modelLocator.HasItem<SampleA>("key2"), "ModelLocator should have the item with key 'key2'");
        }

        [Test]
        public void RemoveItem_ShouldRemoveItemFromModelLocator()
        {
            var context = new SampleContext();
            var genericClass = new GenericClass<SampleA>(context);

            var item = new SampleA();
            var modelLocator = genericClass.ModelLocator;
            Assert.IsNotNull(modelLocator, "ModelLocator should not be null");

            modelLocator.AddItem(item);
            modelLocator.RemoveItem<SampleA>();

            Assert.IsFalse(modelLocator.HasItem<SampleA>(), "ModelLocator should not have the item after removal");
        }

    }
}
