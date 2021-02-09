using Assets.Code.Bubbles;
using Assets.Code.Hybrid;
using Unity.Transforms;

namespace Assets.Code.Grid.Hybrid
{
    internal class Cell : HybridMonoBase
    {
        public void CreateAndSetupCellEntity(ICellData cellData)
        {
            CreateEntity();
            entityManager.AddComponentData(entity, new Translation { Value = cellData.Position });
            entityManager.AddComponentData(entity, new CellCmp { Diameter = cellData.Diameter });
            entityManager.AddComponentData(entity, new Scale { Value = cellData.Diameter });
            entityManager.AddComponentData(Entity, new SpawnBubbleCmp());
        }
    }
}
