using Unity.Mathematics;
using UnityEngine;

namespace Assets.Code.Grid
{
    internal class GridEx
    {
        public static T[] GetCellsPositionsInARow<T>(float radius, float3 startingCenteredPosition, int numberOfCells) where T : IGridPosition, new()
        {
            float diameter = radius * 2;

            Vector3 radiusShiftRight = new Vector3(radius, 0, 0);
            Vector3 diameterShiftRight = radiusShiftRight * 2;

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
