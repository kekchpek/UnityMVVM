using UnityEngine;

namespace UnityMVVM.ViewModelCore.PrefabsProvider
{
    /// <summary>
    /// The provider for view prefabs, that should be used in case of missing explicit prefab getter.
    /// </summary>
    public interface IViewsPrefabsProvider
    {
        /// <summary>
        /// Returns a prefab for specified view name.
        /// </summary>
        /// <param name="viewName">The name of a view to obtain prefab.</param>
        /// <returns>The view prefab.</returns>
        GameObject GetViewPrefab(string viewName);
    }
}