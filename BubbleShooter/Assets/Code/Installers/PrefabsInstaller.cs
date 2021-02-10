using Assets.Code.Bubbles.Hybrid;
using Assets.Code.Grid.Hybrid;
using UnityEngine;
using Zenject;

namespace Assets.Code.Installers
{
    [CreateAssetMenu(menuName = "Installers/Prefabs")]
    internal class PrefabsInstaller : ScriptableObjectInstaller
    {
        [SerializeField]
        private Cell cellPrefab;

        [SerializeField]
        private Bubble bubblePrefab;

        public override void InstallBindings()
        {
            Container.Bind<Cell>().FromInstance(cellPrefab);
            Container.Bind<Bubble>().FromInstance(bubblePrefab);
        }
    }
}
