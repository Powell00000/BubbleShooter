using Assets.Code.Mono;
using UnityEngine;
using Zenject;

namespace Assets.Code.Installers
{
    [CreateAssetMenu(menuName = "Installers/Prefabs")]
    class PrefabsInstaller : ScriptableObjectInstaller
    {
        [SerializeField]
        GridCellBehaviour cellPrefab;

        public override void InstallBindings()
        {
            Container.Bind<GridCellBehaviour>().FromInstance(cellPrefab);
        }
    }
}
