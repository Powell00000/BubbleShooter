using UnityEngine;
using Zenject;

namespace Assets.Code.Installers
{
    [CreateAssetMenu(menuName = "Installers/Settings")]
    internal class SettingsInstaller : ScriptableObjectInstaller
    {
        [SerializeField]
        private GameSettings gameSettings;

        public override void InstallBindings()
        {
            Container.Bind<GameSettings>().FromInstance(gameSettings);
        }
    }
}
