using Assets.Code.DOTS;
using Assets.Code.Grid.Hybrid;
using Unity.Entities;

namespace Assets.Code.Grid.Spawn.Hybrid
{
    internal class RowSpawnSystem : CellSpawnSystem
    {
        [Zenject.Inject]
        private Cell gridCellPrefab = null;

        protected override void OnCreate()
        {
            base.OnCreate();
            RequireSingletonForUpdate<SpawnRowCmp>();
        }

        protected override void OnUpdate()
        {
            SpawnRowCmp spawnRowCmp = GetSingleton<SpawnRowCmp>();
            SpawnCellsInRow(gridCellPrefab, spawnRowCmp.CellDiameter, spawnRowCmp.Position, spawnRowCmp.CellCount);

            var beginSimBuffer = beginSimulationBuffer.CreateCommandBuffer();

            beginSimBuffer.DestroyEntity(GetSingletonEntity<SpawnRowCmp>());
            beginSimBuffer.CreateEntity(EntityManager.CreateArchetype(Archetypes.RowSpawned));
        }
    }
}
