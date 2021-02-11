using Assets.Code.DOTS;
using Assets.Code.Grid.Cells.Hybrid;
using Unity.Entities;
using Unity.Mathematics;

namespace Assets.Code.Grid.Spawn.Hybrid
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    internal class SpawnGridSystem : CellSpawnSystem
    {
        [Zenject.Inject]
        private Cell gridCellPrefab;

        protected override void OnUpdate()
        {

        }

        public void SpawnInitialBoard(float3 position, int cellCount, float cellDiameter, GameManager gameManager)
        {
            for (int rowCount = 0; rowCount < GameManager.MaxRowsCount; rowCount++)
            {
                gameManager.IsEvenRow = rowCount % 2 == 0;

                float3 spawnPosition = position + new float3(0, -rowCount * cellDiameter, 0);
                int cells = gameManager.IsEvenRow == true ? cellCount : cellCount - 1;
                SpawnCellsInRow(gridCellPrefab, cellDiameter, spawnPosition, cells, rowCount);
            }

            var beginInitBuffer = beginInitializationBuffer.CreateCommandBuffer();
            beginInitBuffer.CreateEntity(EntityManager.CreateArchetype(Archetypes.RowSpawned));
        }
    }
}
