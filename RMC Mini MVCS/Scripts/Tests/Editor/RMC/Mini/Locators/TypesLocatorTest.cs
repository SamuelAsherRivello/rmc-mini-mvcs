using NUnit.Framework;
using System;

namespace RMC.Mini.Locators
{
    /// <summary>
    /// I suspect that something was broken with child types or nested types.
    /// </summary>
    [Category("RMC.Mini.Locators")]
    public class TypesLocatorTest
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
        
        public class SampleSubASubB: SampleSubA
        {
        }


        // MY TESTS
        [Test]
        public void HasItem_IsTrue_WhenLocatorIA_AddA_HasA()
        {
            Locator<IA> locator = new Locator<IA>();
            var item = new SampleA();
            locator.AddItem(item);
            Assert.IsTrue(locator.HasItem<SampleA>());
        }
        
        [Test]
        public void HasItem_IsTrue_WhenLocatorA_AddA_HasA()
        {
            Locator<SampleA> locator = new Locator<SampleA>();
            var item = new SampleA();
            locator.AddItem(item);
            Assert.IsTrue(locator.HasItem<SampleA>());
        }
        
        [Test]
        public void HasItem_IsTrue_WhenLocatorSubA_AddSubA_HasSubA()
        {
            Locator<SampleSubA> locator = new Locator<SampleSubA>();
            var item = new SampleSubA();
            locator.AddItem(item);
            Assert.IsTrue(locator.HasItem<SampleSubA>());
        }
        
        [Test]
        public void HasItem_IsTrue_WhenLocatorSubASubB_AddSubASubB_HasSubASubB()
        {
            Locator<SampleSubASubB> locator = new Locator<SampleSubASubB>();
            var item = new SampleSubASubB();
            locator.AddItem(item);
            Assert.IsTrue(locator.HasItem<SampleSubASubB>());
        }
        
        // AI TESTS...
        
        [Test]
        public void HasItem_IsFalse_WhenLocatorIA_AddSubA_HasA()
        {
            Locator<IA> locator = new Locator<IA>();
            var item = new SampleSubA();
            locator.AddItem(item);
            Assert.IsFalse(locator.HasItem<SampleA>());
        }

        [Test]
        public void HasItem_IsTrue_WhenLocatorIA_AddSubA_HasSubA()
        {
            Locator<IA> locator = new Locator<IA>();
            var item = new SampleSubA();
            locator.AddItem(item);
            Assert.IsTrue(locator.HasItem<SampleSubA>());
        }
        
        [Test]
        public void HasItem_IsFalse_WhenLocatorA_AddSubASubB_HasSubA()
        {
            Locator<SampleA> locator = new Locator<SampleA>();
            var item = new SampleSubASubB();
            locator.AddItem(item);
            Assert.IsFalse(locator.HasItem<SampleSubA>());
        }

        [Test]
        public void HasItem_IsTrue_WhenLocatorSubA_AddSubASubB_HasSubASubB()
        {
            Locator<SampleSubA> locator = new Locator<SampleSubA>();
            var item = new SampleSubASubB();
            locator.AddItem(item);
            Assert.IsTrue(locator.HasItem<SampleSubASubB>());
        }

        [Test]
        public void GetItem_IsNull_WhenLocatorIA_AddSubA_GetA()
        {
            Locator<IA> locator = new Locator<IA>();
            var item = new SampleSubA();
            locator.AddItem(item);
            var retrievedItem = locator.GetItem<SampleA>();
            Assert.IsNull(retrievedItem);
        }

        [Test]
        public void GetItem_IsNotNull_WhenLocatorIA_AddSubA_GetSubA()
        {
            Locator<IA> locator = new Locator<IA>();
            var item = new SampleSubA();
            locator.AddItem(item);
            var retrievedItem = locator.GetItem<SampleSubA>();
            Assert.IsNotNull(retrievedItem);
        }

        [Test]
        public void GetItem_IsNull_WhenLocatorA_AddSubASubB_GetSubA()
        {
            Locator<SampleA> locator = new Locator<SampleA>();
            var item = new SampleSubASubB();
            locator.AddItem(item);
            var retrievedItem = locator.GetItem<SampleSubA>();
            Assert.IsNull(retrievedItem);
        }

        [Test]
        public void GetItem_IsNotNull_WhenLocatorSubA_AddSubASubB_GetSubASubB()
        {
            Locator<SampleSubA> locator = new Locator<SampleSubA>();
            var item = new SampleSubASubB();
            locator.AddItem(item);
            var retrievedItem = locator.GetItem<SampleSubASubB>();
            Assert.IsNotNull(retrievedItem);
        }
    }
}
