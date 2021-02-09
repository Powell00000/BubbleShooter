using Assets.Code.Mono;
using UnityEngine;
using Zenject;

namespace Assets.Code.Installers
{
    internal class GameplayInstaller : MonoInstaller
    {
        [SerializeField]
        private CameraBounds cameraBounds;

        public override void InstallBindings()
        {
            Container.Bind<CameraBounds>().FromInstance(cameraBounds);
        }
    }
}
