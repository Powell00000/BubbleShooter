using Assets.Code.Grid.Cells;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.Code.Grid
{
    internal class GridEx
    {
        public static T[] GetCellsPositionsInARow<T>(float cellDiameter, float3 startingCenteredPosition, int numberOfCells, int rowNumber) where T : ICellData, new()
        {
            float radius = cellDiameter / 2;

            Vector3 radiusShiftRight = new Vector3(radius, 0, 0);
            Vector3 diameterShiftRight = radiusShiftRight * 2;

            float width = cellDiameter * numberOfCells;
            float halfWidth = width / 2;

            Vector3 startingCellPosition = startingCenteredPosition + (float3)Vector3.left * halfWidth;
            startingCellPosition += new Vector3(radius, 0, 0);

            T[] cellRow = new T[numberOfCells];


            for (int cellNumber = 0; cellNumber < numberOfCells; cellNumber++)
            {
                cellRow[cellNumber] = new T
                {
                    Position = startingCellPosition + diameterShiftRight * cellNumber,
                    Diameter = cellDiameter,
                    Row = rowNumber
                };
            }

            return cellRow;
        }
    }
}
