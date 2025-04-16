using UnityEngine;
using UnityMVVM.ViewModelCore.PrefabsProvider;

namespace CCG.Core
{
    public class ResourcesPrefabProvider : IViewsPrefabsProvider
    {
        public GameObject GetViewPrefab(string viewName)
        {
            return Resources.Load<GameObject>($"Prefabs/Views/{viewName}View");
        }
    }
}