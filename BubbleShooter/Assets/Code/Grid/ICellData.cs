using Unity.Mathematics;

namespace Assets.Code.Grid
{
    internal interface ICellData
    {
        float3 Position { get; set; }
        float Diameter { get; set; }
    }
}
