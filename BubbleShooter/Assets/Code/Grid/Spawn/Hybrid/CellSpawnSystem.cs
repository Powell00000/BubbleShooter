using Assets.Code.Grid.Cells;
using Assets.Code.Grid.Cells.Hybrid;
using Unity.Mathematics;

namespace Assets.Code.Grid.Spawn.Hybrid
{
    internal abstract class CellSpawnSystem : SystemBaseWithBarriers
    {
        protected void SpawnCellsInRow(Cell prefab, float cellDiameter, float3 spawnPosition, int cellsCount, int rowNumber = 0)
        {
            CellData[] row = GridEx.GetCellsPositionsInARow<CellData>(cellDiameter, spawnPosition, cellsCount, rowNumber);

            for (int cellNumber = 0; cellNumber < row.Length; cellNumber++)
            {
                Cell cellHybridMono = UnityEngine.Object.Instantiate(prefab).GetComponent<Cell>();
                cellHybridMono.CreateAndSetupCellEntity(row[cellNumber]);
            }
        }
    }
}
