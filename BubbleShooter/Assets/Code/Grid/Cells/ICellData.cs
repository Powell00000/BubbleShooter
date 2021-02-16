using Unity.Mathematics;

namespace Assets.Code.Grid.Cells
{
    internal interface ICellData
    {
        float3 Position { get; set; }
        float Diameter { get; set; }
        int Row { get; set; }
    }
}
