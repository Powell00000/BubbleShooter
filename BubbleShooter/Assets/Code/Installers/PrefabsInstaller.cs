using Assets.Code.Bubbles.Hybrid;
using Assets.Code.Bubbles.Mono;
using Assets.Code.Grid.Cells.Hybrid;
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

        [SerializeField]
        private ShootingBubble shootingBubble;

        public override void InstallBindings()
        {
            Container.Bind<Cell>().FromInstance(cellPrefab);
            Container.Bind<Bubble>().FromInstance(bubblePrefab);
            Container.Bind<ShootingBubble>().FromInstance(shootingBubble);
        }
    }
}
