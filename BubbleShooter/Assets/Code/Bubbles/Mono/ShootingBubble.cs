using Assets.Code.Grid.Hybrid;
using System.Linq;
using UnityEngine;

namespace Assets.Code.Bubbles.Mono
{
    internal class ShootingBubble : MonoBehaviour
    {
        private Vector3 direction;
        private float speed = 10;

        public void SetDirection(Vector3 direction)
        {
            this.direction = direction;
        }

        private void Update()
        {
            float radius = GameManager.CellDiameter / 2;
            float extrapolatedDistance = (direction * Time.deltaTime * speed).magnitude;

            if (Physics.SphereCast(transform.position, radius, direction, out var raycastHit, extrapolatedDistance))
            {
                //during next update we will hit a bubble, so we cast for cell to occupy
                //or we will hit upper wall, and we also need to select a cell to occupy
                if (raycastHit.rigidbody.gameObject.layer == LayerMask.NameToLayer("Bubble") || raycastHit.rigidbody.gameObject.name == "TopWall")
                {
                    GetCellNearPosition(transform.position, radius);
                    //destroy this bubble
                    Destroy(gameObject);
                }
                else
                {
                    Vector3 sphereContactPoint = raycastHit.point + raycastHit.normal * (radius / 2);
                    direction = Vector3.Reflect(direction, raycastHit.normal);
                }
            }
            transform.position += direction * Time.deltaTime * speed;

        }
        public static Cell GetCellNearPosition(Vector3 position, float radius)
        {
            Collider[] overlappingCells = Physics.OverlapSphere(position, radius, LayerMask.GetMask("Cell"));
            var sortedCells = overlappingCells.OrderBy((cell) => (cell.transform.position - position).sqrMagnitude).ToArray();
            return sortedCells[0].attachedRigidbody.GetComponent<Cell>();
        }
    }
}
