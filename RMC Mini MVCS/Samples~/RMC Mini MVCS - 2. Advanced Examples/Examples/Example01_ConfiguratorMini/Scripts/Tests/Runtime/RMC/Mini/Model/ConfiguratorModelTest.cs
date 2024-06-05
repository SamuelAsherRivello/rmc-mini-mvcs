using NUnit.Framework;
using RMC.Core.Architectures.Mini.Context;
using RMC.MiniMvcs.Samples.Configurator.Mini.Model;
using RMC.MiniMvcs.Samples.Configurator.Mini.Model.Data;

namespace RMC.MiniMvcs.Samples.Configurator.Mini.Model
{
    [TestFixture]
    [Category ("RMC.Mini.Configurator")]
    public class ConfiguratorModelTest
    {
        private ConfiguratorModel _model;
        private IContext _context;

        [SetUp]
        public void SetUp()
        {
            _model = new ConfiguratorModel();
            _context = new BaseContext();
        }

        [Test]
        public void Initialize_SetsDefaults()
        {
            // Arrange
            Assert.IsFalse(_model.IsInitialized);
            
            // Act
            _model.Initialize(_context);
            
            // Assert
            Assert.IsTrue(_model.IsInitialized);
            Assert.IsFalse(_model.HasLoadedService.Value);
            Assert.IsNull(_model.CharacterData.Value);
            Assert.IsNull(_model.EnvironmentData.Value);
        }

        [Test]
        public void GameMode_InitialValue_IsDefault()
        {
            // Assert
            Assert.AreEqual(GameMode.Menu, _model.HasBackNavigation.Value);
        }

        [Test]
        public void HasLoadedService_InitialValue_IsFalse()
        {
            // Assert
            Assert.IsFalse(_model.HasLoadedService.Value);
        }

        [Test]
        public void CharacterData_InitialValue_IsNull()
        {
            // Assert
            Assert.IsNull(_model.CharacterData.Value);
        }

        [Test]
        public void EnvironmentData_InitialValue_IsNull()
        {
            // Assert
            Assert.IsNull(_model.EnvironmentData.Value);
        }

        [Test]
        public void SetHasBackNavigation_UpdatesValue()
        {
            // Act
            _model.HasBackNavigation.Value = false;
            
            // Assert
            Assert.AreEqual(false,_model.HasBackNavigation.Value);
        }

        [Test]
        public void SetHasLoadedService_UpdatesValue()
        {
            // Act
            _model.HasLoadedService.Value = true;
            
            // Assert
            Assert.IsTrue(_model.HasLoadedService.Value);
        }

        [Test]
        public void SetCharacterData_UpdatesValue()
        {
            // Arrange
            var characterData = new CharacterData();
            
            // Act
            _model.CharacterData.Value = characterData;
            
            // Assert
            Assert.AreEqual(characterData, _model.CharacterData.Value);
        }

        [Test]
        public void SetEnvironmentData_UpdatesValue()
        {
            // Arrange
            var environmentData = new EnvironmentData();
            
            // Act
            _model.EnvironmentData.Value = environmentData;
            
            // Assert
            Assert.AreEqual(environmentData, _model.EnvironmentData.Value);
        }
    }
}

