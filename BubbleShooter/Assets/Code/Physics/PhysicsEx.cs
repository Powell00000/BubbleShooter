using Assets.Code.Bubbles.Hybrid;
using Assets.Code.Grid.Cells.Hybrid;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Code.Physics
{
    internal class PhysicsEx
    {
        public struct CastResult
        {
            public bool SomethingHit;
            public Vector3 ReflectedDirection;
            public Vector3 ContactPoint;
            public Cell FoundCell;
            public bool FinalObjectHit;
        }

        public static CastResult Cast(Vector3 direction, float distance, Vector3 position)
        {
            CastResult castResult = new CastResult();

            float radius = GameManager.CellDiameter / 2;
            radius *= 0.9f;

            castResult.SomethingHit = UnityEngine.Physics.SphereCast(position, radius, direction, out var raycastHit, distance, LayerMask.GetMask("Default", "Bubble"));
            if (castResult.SomethingHit)
            {
                Vector3 sphereContactPoint = raycastHit.point + raycastHit.normal * radius;
                castResult.ContactPoint = sphereContactPoint;

                castResult.ReflectedDirection = Vector3.Reflect(direction, raycastHit.normal);
                //during next update we will hit a bubble, so we cast for cell to occupy
                //or we will hit upper wall, and we also need to select a cell to occupy
                if (raycastHit.rigidbody.gameObject.layer == LayerMask.NameToLayer("Bubble") || raycastHit.rigidbody.gameObject.name == "TopWall")
                {
                    castResult.FinalObjectHit = true;
                    Cell cell = GetClosestCell(castResult.ContactPoint, radius);
                    castResult.FoundCell = cell;
                }
            }
            return castResult;
        }

        public static Cell GetClosestCell(Vector3 position, float radius)
        {
            Collider[] overlappingCells = UnityEngine.Physics.OverlapSphere(position, radius, LayerMask.GetMask("Cell"));
            var sortedCells = overlappingCells.OrderBy((cell) => (cell.transform.position - position).sqrMagnitude).ToArray();
            Cell closestCell = sortedCells.Length > 0 && sortedCells[0] != null ? sortedCells[0].attachedRigidbody.GetComponent<Cell>() : null;
            return closestCell;
        }

        public static Bubble[] GetNeighbouringBubbles(Bubble bubble)
        {
            Collider[] overlappingCells = new Collider[6];
            int cellsCount = UnityEngine.Physics.OverlapSphereNonAlloc(bubble.transform.position, GameManager.CellDiameter, overlappingCells, LayerMask.GetMask("Bubble"));
            List<Bubble> bubbles = new List<Bubble>(cellsCount);
            for (int i = 0; i < cellsCount; i++)
            {
                var bubbleComponent = overlappingCells[i].attachedRigidbody.GetComponent<Bubble>();
                if (bubbleComponent != bubble)
                {
                    bubbles.Add(bubbleComponent);
                }
            }
            return bubbles.ToArray();
        }

        public static Cell[] GetNeighbouringCells(Vector3 position)
        {
            Collider[] overlappingCells = UnityEngine.Physics.OverlapSphere(position, GameManager.CellDiameter, LayerMask.GetMask("Cell"));
            List<Cell> bubbles = new List<Cell>(overlappingCells.Length);
            for (int i = 0; i < overlappingCells.Length; i++)
            {
                bubbles.Add(overlappingCells[i].attachedRigidbody.GetComponent<Cell>());
            }
            return bubbles.ToArray();
        }
    }
}
