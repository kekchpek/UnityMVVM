using UnityEngine;
using UnityMVVM.DI;
using UnityMVVM.DI.Config;
using Zenject;

namespace CCG.Core
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private Transform _3dRoot;
        [SerializeField] private Transform _uiRoot;
        [SerializeField] private Transform _popupRoot;
        
        public override void InstallBindings()
        {
            Container.UseAsMvvmContainer(new []
            {
                (ViewLayerIds.Main3d, _3dRoot),
                (ViewLayerIds.MainUI, _uiRoot),
                (ViewLayerIds.Popup, _popupRoot)
            });
            Container.Install<CoreInstaller>();
        }
    }
}