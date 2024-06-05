using System;
using System.Reflection;
using NUnit.Framework;
using RMC.Core.Architectures.Mini.Context;
using RMC.MiniMvcs.Samples.Configurator.Mini.Model;
using RMC.MiniMvcs.Samples.Configurator.Mini.Service;
using RMC.MiniMvcs.Samples.Configurator.Mini.View;
using UnityEngine;
using UnityEngine.Events;

namespace RMC.MiniMvcs.Samples.Configurator.Mini.Controller
{
    
    [TestFixture]
    [Category ("RMC.Mini.Configurator")]
    public class HudControllerTest
    {
        private HudController _hudController;
        private ConfiguratorModel _model;
        private HudView _view;
        private ConfiguratorService _service;
        private IContext _context;

        [SetUp]
        public void SetUp()
        {
            _model = new ConfiguratorModel();
            _view = new GameObject().AddComponent<HudView>();
            _service = new ConfiguratorService();

            _context = new BaseContext();

            _hudController = new HudController(_model, _view, _service);
        }

        [Test]
        public void Initialize_SetsDefaults()
        {
            // Arrange
            Assert.IsFalse(_hudController.IsInitialized);

            // Act
            _hudController.Initialize(_context);

            // Assert
            Assert.IsTrue(_hudController.IsInitialized);
            Assert.IsTrue(HasListeners(_view.OnBack));
        }
        
        private bool HasListeners(UnityEvent unityEvent)
        {
            var field = typeof(UnityEventBase).GetField("m_Calls", BindingFlags.NonPublic | BindingFlags.Instance);
            var calls = field.GetValue(unityEvent);
            var countProperty = calls.GetType().GetProperty("Count");
            int count = (int)countProperty.GetValue(calls);
            return count > 0;
        }

        [Test]
        public void View_OnBack_ThrowsException_WhenNotInitialized()
        {
            // Arrange
            Assert.IsFalse(_hudController.IsInitialized);

            // Act & Assert
            Assert.Throws<Exception>(() => _view.OnBack.Invoke());
        }

        [TearDown]
        public void TearDown()
        {
            UnityEngine.Object.DestroyImmediate(_view.gameObject);
        }
    }
}
