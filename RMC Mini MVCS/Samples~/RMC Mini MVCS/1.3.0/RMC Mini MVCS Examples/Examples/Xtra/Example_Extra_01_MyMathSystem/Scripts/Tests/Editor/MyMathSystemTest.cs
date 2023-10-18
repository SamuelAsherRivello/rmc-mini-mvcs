using NUnit.Framework;

namespace RMC.IntroToUnity.Samples.MyMathSystem
{
    /// <summary>
    /// This Unit Test validates that code executes as expected.
    /// </summary>
    [Category ("RMC.Mini.Samples.MyMathSystem")]
    public class MyMathSystemTest
    {
        [Test]
        public void Add_ResultIs15_When5And10()
        {
            // Arrange
            MyMathSystem myMathSystem = new MyMathSystem();
            
            // Act
            int sum = myMathSystem.Add(5, 10);
            
            // Assert
            Assert.That(sum, Is.EqualTo(15));
        }
        
        [Test]
        public void Subtract_ResultIs5_When10And5()
        {
            // Arrange
            MyMathSystem myMathSystem = new MyMathSystem();
            
            // Act
            int sum = myMathSystem.Subtract(10, 5);
            
            // Assert
            Assert.That(sum, Is.EqualTo(5));
        }
        
        static int[] valuesA = new int[] { -1, -2, -3, 0, 1, 2, 3 };
        static int[] valuesB = new int[] { -1, -2, -3, 0, 1, 2, 3 };
        
        [Test]
        public void Add_ResultIsCorrect_WhenValues(
            [ValueSource("valuesA")] int valuesA, 
            [ValueSource("valuesB")] int valuesB )
        {
            // Arrange
            MyMathSystem myMathSystem = new MyMathSystem();
            
            // Act
            int sum = myMathSystem.Add(valuesA, valuesB);
            
            // Assert
            Assert.That(sum, Is.EqualTo(valuesA + valuesB));
        }
    }
}