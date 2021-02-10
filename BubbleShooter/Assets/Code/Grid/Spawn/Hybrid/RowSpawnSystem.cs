using Assets.Code.DOTS;
using Assets.Code.Grid.Hybrid;
using Unity.Entities;

namespace Assets.Code.Grid.Spawn.Hybrid
{
    internal class RowSpawnSystem : SystemBaseWithBarriers
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
            CellData[] row = GridEx.GetCellsPositionsInARow<CellData>(spawnRowCmp.CellDiameter, spawnRowCmp.Position, spawnRowCmp.CellCount);

            for (int cellNumber = 0; cellNumber < row.Length; cellNumber++)
            {
                Cell cellHybridMono = UnityEngine.Object.Instantiate(gridCellPrefab).GetComponent<Cell>();
                cellHybridMono.CreateAndSetupCellEntity(row[cellNumber]);
            }

            var beginSimBuffer = beginSimulationBuffer.CreateCommandBuffer();

            beginSimBuffer.DestroyEntity(GetSingletonEntity<SpawnRowCmp>());
            beginSimBuffer.CreateEntity(EntityManager.CreateArchetype(Archetypes.RowSpawned));
        }
    }
}
