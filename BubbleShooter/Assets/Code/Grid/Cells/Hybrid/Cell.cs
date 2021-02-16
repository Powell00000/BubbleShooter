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
            entityManager.AddComponentData(entity, new Row.RowSharedCmp { RowNumber = cellData.Row });
            entityManager.SetComponentData(entity, new Scale { Value = cellData.Diameter });
            entityManager.AddComponentObject(Entity, transform);
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (!Application.isPlaying)
            {
                return;
            }

            Handles.Label(transform.position, $"Empty: {entityManager.GetComponentData<CellCmp>(entity).IsEmpty}");
        }
#endif
    }
}
