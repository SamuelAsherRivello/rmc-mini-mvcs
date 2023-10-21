using JetBrains.Annotations;
using System;

namespace Unity.Tutorials.Core.Editor
{
    /// <summary>
    /// This is a template class for new Models
    /// </summary>
    [Serializable]
    internal class EmptyModel : IModel
    {
        /// <inheritdoc />
        public event Action StateChanged;

        /// <inheritdoc />
        public void OnStart() { }

        /// <inheritdoc />
        public void OnStop() { }

        /// <inheritdoc />
        public void RestoreState([NotNull] IWindowCache cache)
        {
            StateChanged?.Invoke();
        }

        /// <inheritdoc />
        public void SaveState([NotNull] IWindowCache cache) { }
    }
}
