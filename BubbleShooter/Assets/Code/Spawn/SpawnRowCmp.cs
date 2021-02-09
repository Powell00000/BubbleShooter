using Unity.Entities;
using Unity.Mathematics;

namespace Assets.Code.Spawn
{
    internal struct SpawnRowCmp : IComponentData
    {
        public float3 Position;
        public int CellCount;
        public float CellRadius;
    }
}
