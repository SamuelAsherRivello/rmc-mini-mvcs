using NUnit.Framework;
using System;

namespace RMC.Mini.Locators
{
    

    [Category("RMC.Mini.Locators")]
    public class LocatorTests
    {
        
        public interface ISampleBase : IDisposable
        {
        }

        public class SampleItemA : ISampleBase
        {
            public void Dispose()
            {
                // TODO release managed resources here
            }
        }

        public class SampleItemB : ISampleBase
        {
            public void Dispose()
            {
                // TODO release managed resources here
            }
        }
        
        
        private Locator<ISampleBase> _locator;

        [SetUp]
        public void SetUp()
        {
            _locator = new Locator<ISampleBase>();
        }

        [Test]
        public void AddItem_ShouldAddItem()
        {
            var item = new SampleItemA();
            _locator.AddItem(item);

            Assert.IsTrue(_locator.HasItem<SampleItemA>());
        }

        [Test]
        public void GetItem_ShouldReturnCorrectItem()
        {
            var item = new SampleItemA();
            _locator.AddItem(item);

            var retrievedItem = _locator.GetItem<SampleItemA>();

            Assert.AreSame(item, retrievedItem);
        }

        [Test]
        public void RemoveItem_ShouldRemoveItem()
        {
            var item = new SampleItemA();
            _locator.AddItem(item);
            _locator.RemoveAndDisposeItem<SampleItemA>();

            Assert.IsFalse(_locator.HasItem<SampleItemA>());
        }

        [Test]
        public void AddItem_WithSameType_ShouldThrowException()
        {
            var item1 = new SampleItemA();
            var item2 = new SampleItemA();
            _locator.AddItem(item1);

            Assert.Throws<Exception>(() => _locator.AddItem(item2));
        }

        [Test]
        public void AddItem_WithDifferentKeys_ShouldAddItems()
        {
            var item1 = new SampleItemA();
            var item2 = new SampleItemA();
            _locator.AddItem(item1, "key1");
            _locator.AddItem(item2, "key2");

            Assert.IsTrue(_locator.HasItem<SampleItemA>("key1"));
            Assert.IsTrue(_locator.HasItem<SampleItemA>("key2"));
        }

        [Test]
        public void GetItem_WithInvalidKey_ShouldReturnNull()
        {
            var item = new SampleItemA();
            _locator.AddItem(item, "key1");

            var retrievedItem = _locator.GetItem<SampleItemA>("invalidKey");

            Assert.IsNull(retrievedItem);
        }

        [Test]
        public void Dispose_ShouldClearAllItems()
        {
            var item = new SampleItemA();
            _locator.AddItem(item);
            _locator.Dispose();

            Assert.IsFalse(_locator.HasItem<SampleItemA>());
        }

        [Test]
        public void Reset_ShouldClearAllItems()
        {
            var item = new SampleItemA();
            _locator.AddItem(item);
            _locator.Reset();

            Assert.IsFalse(_locator.HasItem<SampleItemA>());
        }

        [TearDown]
        public void TearDown()
        {
            _locator.Dispose();
        }
    }
}
