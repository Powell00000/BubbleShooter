using Assets.Code.Bubbles;
using Assets.Code.Grid.Cells;
using Assets.Code.Hybrid;
using Unity.Transforms;
using UnityEngine;

namespace Assets.Code.Grid.Hybrid
{
    internal class Cell : HybridMonoBase
    {
        public bool gizmos = false;
        public void CreateAndSetupCellEntity(ICellData cellData)
        {
            CreateEntity();
            entityManager.AddComponentData(entity, new Translation { Value = cellData.Position });
            entityManager.AddComponentData(entity, new CellCmp { Diameter = cellData.Diameter });
            entityManager.AddSharedComponentData(entity, new Row.RowSharedCmp { RowNumber = cellData.Row, MaxRowNumber = GameManager.MaxRowsCount });
            entityManager.AddComponentData(entity, new Scale { Value = cellData.Diameter });
            transform.localScale *= cellData.Diameter;
            entityManager.AddComponentData(entity, new CopyTransformToGameObject());
            entityManager.AddComponentObject(Entity, transform);
            entityManager.AddComponentData(Entity, new SpawnBubbleCmp());
        }

        private void LateUpdate()
        {
            gizmos = false;
        }

        private void OnDrawGizmos()
        {
            if (gizmos)
            {
                Gizmos.DrawWireSphere(transform.position, 1);
            }
        }
    }
}
