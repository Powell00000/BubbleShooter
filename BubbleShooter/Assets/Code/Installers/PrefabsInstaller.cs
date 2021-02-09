using Assets.Code.Bubbles;
using Assets.Code.Bubbles.Hybrid;
using Assets.Code.Mono;
using UnityEngine;
using Zenject;

namespace Assets.Code.Installers
{
    [CreateAssetMenu(menuName = "Installers/Prefabs")]
    internal class PrefabsInstaller : ScriptableObjectInstaller
    {
        [SerializeField]
        private GridCellBehaviour cellPrefab;

        [SerializeField]
        private Bubble bubblePrefab;

        public override void InstallBindings()
        {
            Container.Bind<GridCellBehaviour>().FromInstance(cellPrefab);
            Container.Bind<Bubble>().FromInstance(bubblePrefab);
        }
    }
}
