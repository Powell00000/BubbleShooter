using Unity.Mathematics;

namespace Assets.Code.Grid
{
    internal struct CellData : ICellData
    {
        public float3 Position { get; set; }
        public float Diameter { get; set; }
    }
}
