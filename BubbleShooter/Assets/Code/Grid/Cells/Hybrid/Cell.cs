using Assets.Code.Bubbles.Connections;
using Assets.Code.Hybrid;
using Unity.Transforms;
using UnityEditor;
using UnityEngine;

namespace Assets.Code.Grid.Cells.Hybrid
{
    internal class Cell : HybridMonoBase
    {
        [SerializeField]
        private SphereCollider sphereCollider;

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

        private void OnDrawGizmos()
        {
            Handles.Label(transform.position, $"Empty: {entityManager.GetComponentData<CellCmp>(entity).IsEmpty}");
            Handles.Label(transform.position + Vector3.down * 0.1f, $"Has connection: {entityManager.HasComponent<HasConnectionWithTopRowTagCmp>(entity)}");

        }
    }
}
