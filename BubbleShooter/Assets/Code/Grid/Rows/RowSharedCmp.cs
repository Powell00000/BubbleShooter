using Unity.Entities;

namespace Assets.Code.Grid.Row
{
    public struct RowSharedCmp : ISharedComponentData
    {
        public int RowNumber;
        //public bool IsOdd => RowNumber % 2 != 0;
    }
}