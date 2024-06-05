using System;
using NUnit.Framework;
using RMC.Core.Architectures.Mini.Model;
using RMC.Core.Experimental;


namespace RMC.Core.Architectures.Mini.Context
{
    /// <summary>
    /// Testing after a bug was found with <see cref="ModelLocator"/>.
    ///
    /// KEEP few tests here. Keep most in <see cref="RMC.Core.Architectures.Mini.Context.LocatorTest"/>
    /// </summary>
    [Category ("RMC.Mini")]
    public class ModelLocatorTest
    {
        public class SubclassOfIModel : IModel
        {
            public bool IsInitialized { get; }
            public IContext Context { get; }
            public void Initialize(IContext context)
            {
                
            }

            public void RequireIsInitialized()
            {
               
            }
        }
        
        public class SubclassOfBaseModel : BaseModel
        {
            public override void Initialize(IContext context) 
            {
                if (!IsInitialized)
                {
                    base.Initialize(context);

                }
            }
        }


        [TearDown]
        public void TearDown()
        {
            if (ContextLocator.Instance.HasItem<Context>())
            {
                ContextLocator.Instance.RemoveItem<Context>();
            }
        }
        
        [Test]
        public void GetItem_IsNotNull_WhenAddItem_SubclassOfIModel()
        {
            // Arrange
            ModelLocator modelLocator = new ModelLocator();
            SubclassOfIModel subclassOfIModel01 = new SubclassOfIModel();
            Context context = new Context();
            subclassOfIModel01.Initialize(context);
            modelLocator.AddItem(subclassOfIModel01);
                  
            // Act
            var found = modelLocator.GetItem<SubclassOfIModel>();
            
            // Assert
            Assert.That(found, Is.Not.Null);
        }
        
        [Test]
        public void GetItem_IsNotNull_WhenAddItemWithKey_SubclassOfIModel()
        {
            // Arrange
            ModelLocator modelLocator = new ModelLocator();
            SubclassOfIModel subclassOfIModel01 = new SubclassOfIModel();
            Context context = new Context();
            subclassOfIModel01.Initialize(context);
            modelLocator.AddItem(subclassOfIModel01, "01");
                  
            // Act
            var found = modelLocator.GetItem<SubclassOfIModel>("01");
            
            // Assert
            Assert.That(found, Is.Not.Null);
        }
        
        [Test]
        public void GetItem_IsNotNull_WhenAddItem_SubclassOfBaseModel()
        {
            // Arrange
            ModelLocator modelLocator = new ModelLocator();
            SubclassOfBaseModel subclassOfBaseModel = new SubclassOfBaseModel();
            Context context = new Context();
            subclassOfBaseModel.Initialize(context);
            modelLocator.AddItem(subclassOfBaseModel);
                  
            // Act
            var found = modelLocator.GetItem<SubclassOfBaseModel>();
            
            // Assert
            Assert.That(found, Is.Not.Null);
        }
        
        [Test]
        public void GetItem_IsNotNull_WhenAddItemWithKey_SubclassOfBaseModel()
        {
            // Arrange
            ModelLocator modelLocator = new ModelLocator();
            SubclassOfBaseModel subclassOfBaseModel = new SubclassOfBaseModel();
            Context context = new Context();
            subclassOfBaseModel.Initialize(context);
            modelLocator.AddItem(subclassOfBaseModel, "01");
                  
            // Act
            var found = modelLocator.GetItem<SubclassOfBaseModel>("01");
            
            // Assert
            Assert.That(found, Is.Not.Null);
        }

        
        [Test]
        public void GetItem_IsNotNull_WhenAddItem_SubclassOfBaseModel_TypedInterface()
        {
            // Arrange
            ModelLocator modelLocator = new ModelLocator();
            IModel subclassOfBaseModel = new SubclassOfBaseModel();
            Context context = new Context();
            subclassOfBaseModel.Initialize(context);
            modelLocator.AddItem(subclassOfBaseModel);
                  
            // Act
            var found = modelLocator.GetItem<SubclassOfBaseModel>();
            
            // Assert
            Assert.That(found, Is.Not.Null);
        }
        
        [Test]
        public void GetItem_IsNotNull_WhenAddItemWithKey_SubclassOfBaseModel_TypedInterface()
        {
            // Arrange
            ModelLocator modelLocator = new ModelLocator();
            IModel subclassOfBaseModel = new SubclassOfBaseModel();
            Context context = new Context();
            subclassOfBaseModel.Initialize(context);
            modelLocator.AddItem(subclassOfBaseModel, "01");
                  
            // Act
            var found = modelLocator.GetItem<SubclassOfBaseModel>("01");
            
            // Assert
            Assert.That(found, Is.Not.Null);
        }
    }
}
