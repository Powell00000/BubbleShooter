using Assets.Code.Grid.Cells.Hybrid;
using System.Linq;
using UnityEngine;

namespace Assets.Code.PhysicsEx
{
    internal class PhysicsEx
    {
        public static bool TryCastForCell(Vector3 direction, float distance, Vector3 position, out Vector3 reflectedDirection, out Vector3 contactPoint, out Cell foundCell)
        {
            foundCell = null;
            reflectedDirection = direction;
            contactPoint = Vector3.zero;

            float radius = GameManager.CellDiameter / 2;
            //radius *= 0.6f;

            if (Physics.SphereCast(position, radius, direction, out var raycastHit, distance, LayerMask.GetMask("Default", "Bubble")))
            {
                Vector3 sphereContactPoint = raycastHit.point + raycastHit.normal * radius;
                contactPoint = sphereContactPoint;

                reflectedDirection = Vector3.Reflect(direction, raycastHit.normal);
                //during next update we will hit a bubble, so we cast for cell to occupy
                //or we will hit upper wall, and we also need to select a cell to occupy
                if (raycastHit.rigidbody.gameObject.layer == LayerMask.NameToLayer("Bubble") || raycastHit.rigidbody.gameObject.name == "TopWall")
                {
                    Cell cell = GetCellNearPosition(contactPoint, radius);
                    foundCell = cell;
                    if (foundCell != null)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static Cell GetCellNearPosition(Vector3 position, float radius)
        {
            Collider[] overlappingCells = Physics.OverlapSphere(position, radius, LayerMask.GetMask("Cell"));
            var sortedCells = overlappingCells.OrderBy((cell) => (cell.transform.position - position).sqrMagnitude).ToArray();
            Cell closestCell = sortedCells.Length > 0 && sortedCells[0] != null ? sortedCells[0].attachedRigidbody.GetComponent<Cell>() : null;
            return closestCell;
        }
    }
}
