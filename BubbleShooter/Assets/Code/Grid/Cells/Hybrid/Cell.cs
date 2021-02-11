using Assets.Code.Hybrid;
using Unity.Transforms;
using UnityEngine;

namespace Assets.Code.Grid.Cells.Hybrid
{
    internal class Cell : HybridMonoBase
    {
        public bool gizmos = false;

        [SerializeField]
        private SpriteRenderer spriteRenderer;
        public void CreateAndSetupCellEntity(ICellData cellData)
        {
            CreateEntity();
            entityManager.AddComponentData(entity, new Translation { Value = cellData.Position });
            entityManager.AddComponentData(entity, new CellCmp { Diameter = cellData.Diameter });
            entityManager.AddSharedComponentData(entity, new Row.RowSharedCmp { RowNumber = cellData.Row });
            entityManager.SetComponentData(entity, new Scale { Value = cellData.Diameter });
            entityManager.AddComponentObject(Entity, transform);
        }

        private void Update()
        {
            spriteRenderer.color = gizmos ? Color.green : Color.gray;
        }
    }
}
