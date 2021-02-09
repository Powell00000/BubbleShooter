using Unity.Entities;
using Zenject;

namespace Assets.Code.Installers
{
    class InjectWorldSystems : MonoInstaller
    {
        public override void InstallBindings()
        {
            foreach (var system in World.DefaultGameObjectInjectionWorld.Systems)
            {
                Container.Inject(system);
            }
        }
    }
}
