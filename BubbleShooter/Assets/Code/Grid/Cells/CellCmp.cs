using Unity.Entities;

namespace Assets.Code.Grid.Cells
{
    [GenerateAuthoringComponent]
    public struct CellCmp : IComponentData
    {
        public Entity OccupyingEntity;
        public bool IsEmpty => OccupyingEntity == Entity.Null;
        public float Diameter;
    }
}