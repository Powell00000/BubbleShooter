using Assets.Code.DOTS;
using Assets.Code.Grid.Hybrid;
using Unity.Mathematics;

namespace Assets.Code.Grid.Spawn.Hybrid
{
    internal class InitialSpawnSystem : CellSpawnSystem
    {
        [Zenject.Inject]
        private Cell gridCellPrefab;

        protected override void OnUpdate()
        {

        }

        public void SpawnInitialBoard(int rowsCount, float3 position, int cellCount, float cellDiameter, GameManager gameManager)
        {
            for (int rowCount = 0; rowCount < rowsCount; rowCount++)
            {
                float3 spawnPosition = position + new float3(0, -rowCount * cellDiameter, 0);
                int cells = gameManager.IsEvenRow == true ? cellCount : cellCount - 1;
                SpawnCellsInRow(gridCellPrefab, cellDiameter, spawnPosition, cells);
                gameManager.IsEvenRow = !gameManager.IsEvenRow;
            }

            var beginSimBuffer = beginSimulationBuffer.CreateCommandBuffer();
            beginSimBuffer.CreateEntity(EntityManager.CreateArchetype(Archetypes.RowSpawned));
        }
    }
}
