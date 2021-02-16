using Assets.Code.DOTS;
using Assets.Code.Grid.Cells.Hybrid;
using Unity.Entities;

namespace Assets.Code.Grid.Spawn.Hybrid
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
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

            var beginInitBuffer = beginInitializationBuffer.CreateCommandBuffer();
            var endSimBuffer = endSimulationBuffer.CreateCommandBuffer();

            beginInitBuffer.CreateEntity(EntityManager.CreateArchetype(Archetypes.RowSpawned));
            endSimBuffer.DestroyEntity(GetSingletonEntity<SpawnRowCmp>());
        }
    }
}
