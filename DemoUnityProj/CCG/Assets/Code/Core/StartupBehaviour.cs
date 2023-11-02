using CCG.Services.Startup;
using UnityEngine;
using Zenject;

namespace CCG.Core
{
    public class StartupBehaviour : MonoBehaviour
    {
        private IStartupService _startupService;
        
        [Inject]
        public void Construct(IStartupService startupService)
        {
            _startupService = startupService;
        }
        
        private async void Start()
        {
            await _startupService.Startup();
        }
    }
}
