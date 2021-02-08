using Unity.Mathematics;
using UnityEngine;

namespace Assets.Code.Grid
{
    class GridManager
    {
        public static T[] GetRowData<T>(float radius, float3 startingCenteredPosition, int numberOfCells) where T : IGridPosition, new()
        {
            float diameter = radius * 2;

            Vector3 radiusShiftRight = new Vector3(radius, 0, 0);
            Vector3 diameterShiftRight = radiusShiftRight * 2;
            Vector3 diameterShiftVertical = new Vector3(0, -radius * 2, 0);

            float width = diameter * numberOfCells;
            float halfWidth = width / 2;

            Vector3 startingCellPosition = startingCenteredPosition + (float3)Vector3.left * halfWidth;
            startingCellPosition += new Vector3(radius, 0, 0);

            T[] cellRow = new T[numberOfCells];


            for (int cellNumber = 0; cellNumber < numberOfCells; cellNumber++)
            {
                cellRow[cellNumber] = new T { Position = startingCellPosition + diameterShiftRight * cellNumber };
            }

            return cellRow;
        }
    }
}
