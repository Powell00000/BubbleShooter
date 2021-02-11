using Unity.Mathematics;

namespace Assets.Code.Grid.Cells
{
    internal struct CellData : ICellData
    {
        public float3 Position { get; set; }
        public float Diameter { get; set; }
        public int Row { get; set; }
    }
}
