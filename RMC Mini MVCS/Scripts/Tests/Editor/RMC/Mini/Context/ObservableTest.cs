using NUnit.Framework;
using RMC.Core.Observables;

//Keep As:RMC.Mini
namespace RMC.Mini
{
    [Category ("RMC.Mini")]
    public class ObservableTest
    {
        
        
        [Test]
        public void Value_Is10_WhenSet10()
        {
            // Arrange
            Observable<int> observable = new Observable<int>();

            // Act
            observable.Value = 10;

            // Assert
            Assert.That(observable.Value, Is.EqualTo(10));
        }
        
        
        [Test]
        public void OnValueChanged_WasCalledIsTrue_WhenCalled()
        {
            // Arrange
            Observable<int> observable = new Observable<int>();
            bool wasCalled = false;
            observable.OnValueChanged.AddListener((p, c) =>
            {
                wasCalled = true;
            });

            // Act
            observable.Value = 10;

            // Assert
            Assert.That(wasCalled, Is.True);
        }
        
        
        [Test]
        public void OnValueChanged_WasCalledIsTrue_WhenNotifyOnValueChanged()
        {
            // Arrange
            Observable<int> observable = new Observable<int>();
            bool wasCalled = false;
            observable.OnValueChanged.AddListener((p, c) =>
            {
                wasCalled = true;
            });

            // Act
            observable.OnValueChangedRefresh();

            // Assert
            Assert.That(wasCalled, Is.True);
        }
        
        [Test]
        public void OnValueChanged_WasCalledIsTrue_WhenAddListenerWithTrue()
        {
            // Arrange
            Observable<int> observable = new Observable<int>();
            bool wasCalled = false;
            const bool willRefresh = true;
            
            // Act
            observable.OnValueChanged.AddListener((p, c) =>
            {
                wasCalled = true;
                
            }, willRefresh);
            
            // Assert
            Assert.That(wasCalled, Is.True);
        }
        
        [Test]
        public void OnValueChanged_WasCalledIsFalse_WhenAddListenerWithFalse()
        {
            // Arrange
            Observable<int> observable = new Observable<int>();
            bool wasCalled = false;
            const bool willRefresh = false;
            
            // Act
            observable.OnValueChanged.AddListener((p, c) =>
            {
                wasCalled = true;
                
            }, willRefresh);
            
            // Assert
            Assert.That(wasCalled, Is.False);
        }
        
        [Test]
        public void OnValueChanged_ParametersAre5_10_WhenValues5_10()
        {
            // Arrange
            Observable<int> observable = new Observable<int>();
            observable.Value = 5;

            int beforeValue = 0;
            int afterValue = 0;
            observable.OnValueChanged.AddListener((p, c) =>
            {
                beforeValue = p;
                afterValue = c;
            });

            // Act
            observable.Value = 10;

            // Assert
            Assert.That(beforeValue, Is.EqualTo(5));
            Assert.That(afterValue, Is.EqualTo(10));
        }
        
        
    }
}
