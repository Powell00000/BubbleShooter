using Assets.Code.Mono;
using UnityEngine;
using Zenject;

namespace Assets.Code.Installers
{
    internal class GameplayInstaller : MonoInstaller
    {
        [SerializeField]
        private GameManager gameManager;

        [SerializeField]
        private CameraBounds cameraBounds;

        [SerializeField]
        private Cannon cannon;

        public override void InstallBindings()
        {
            Container.Bind<GameManager>().FromInstance(gameManager);
            Container.Bind<CameraBounds>().FromInstance(cameraBounds);
            Container.Bind<Cannon>().FromInstance(cannon);
        }
    }
}
