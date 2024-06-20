using System;
using NUnit.Framework;
using RMC.Mini.Samples.Configurator.Mini.Model;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace RMC.Mini.Samples.Configurator.Mini.View
{
    [TestFixture]
    [Category ("RMC.Mini.Configurator")]
    public class HudViewTest
    {
        private HudView _hudView;
        private ConfiguratorModel _model;
        private IContext _context;
        private UIDocument _uiDocument;
        private VisualElement _rootVisualElement;
        private Label _statusLabel;
        private Button _backButton;
        private Button _developerConsoleButton;

        [SetUp]
        public void SetUp()
        {
            _hudView = new GameObject().AddComponent<HudView>();

            _model = new ConfiguratorModel();
            _context = new BaseContext();
            
            _context.ModelLocator.AddItem(_model);

            _rootVisualElement = new VisualElement();
            _statusLabel = new Label { name = "StatusLabel" };
            _backButton = new Button { name = "BackButton" };
            _developerConsoleButton = new Button { name = "DeveloperConsoleButton" };

            _rootVisualElement.Add(_statusLabel);
            _rootVisualElement.Add(_backButton);
            _rootVisualElement.Add(_developerConsoleButton);

            var uiDocument = new GameObject().AddComponent<UIDocument>();
            SetPrivateField(uiDocument, "m_RootVisualElement", _rootVisualElement);
            SetPrivateField(_hudView, "_uiDocument", uiDocument);
        }
        
        private void SetPrivateField(object obj, string fieldName, object value)
        {
            var field = obj.GetType().GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            field.SetValue(obj, value);
        }

        [Test]
        public void Initialize_SetsDefaults()
        {
            // Arrange
            Assert.IsFalse(_hudView.IsInitialized);

            // Act
            _hudView.Initialize(_context);

            // Assert
            Assert.IsTrue(_hudView.IsInitialized);
            Assert.AreEqual(_context, _hudView.Context);
            Assert.IsFalse(_backButton.enabledSelf);
            Assert.IsFalse(_developerConsoleButton.enabledSelf);
        }

        [Test]
        public void Initialize_SetsUIElements()
        {
            // Act
            _hudView.Initialize(_context);

            // Assert
            Assert.AreEqual(SceneManager.GetActiveScene().name, _statusLabel.text);
            Assert.IsFalse(_backButton.enabledSelf);
        }

        [Test]
        public void RequireIsInitialized_ThrowsException_WhenNotInitialized()
        {
            // Assert
            Assert.Throws<Exception>(() => _hudView.RequireIsInitialized());
        }

        [Test]
        public void RequireIsInitialized_DoesNotThrowException_WhenInitialized()
        {
            // Arrange
            _hudView.Initialize(_context);

            // Assert
            Assert.DoesNotThrow(() => _hudView.RequireIsInitialized());
        }


        [Test]
        public void ServiceHasLoaded_OnValueChanged_RefreshesUI()
        {
            // Arrange
            _hudView.Initialize(_context);
            _model.HasLoadedService.Value = true;

            // Act
            _model.HasLoadedService.Value = false;
            _model.HasLoadedService.Value = true;

            // Assert
            Assert.AreEqual(SceneManager.GetActiveScene().name, _statusLabel.text);
            Assert.IsFalse(_backButton.enabledSelf);
            Assert.IsFalse(_developerConsoleButton.enabledSelf);
        }

        [TearDown]
        public void TearDown()
        {
            UnityEngine.Object.DestroyImmediate(_hudView.gameObject);
        }
    }
}
