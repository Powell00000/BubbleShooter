using Unity.Entities;
using Zenject;

namespace Assets.Code.Installers
{
    internal class InjectWorldSystems : MonoInstaller
    {
        public override void InstallBindings()
        {
            if (World.DefaultGameObjectInjectionWorld == null)
            {
                return;
            }

            foreach (var system in World.DefaultGameObjectInjectionWorld.Systems)
            {
                Container.Inject(system);
            }
        }
    }
}
