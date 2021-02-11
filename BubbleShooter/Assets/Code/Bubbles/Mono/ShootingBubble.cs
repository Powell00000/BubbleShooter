using Unity.Entities;
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
            float extrapolatedDistance = (direction * Time.deltaTime * speed).magnitude;
            if (PhysicsEx.PhysicsEx.TryCastForCell(direction, extrapolatedDistance, transform.position, out direction, out var contactPoint, out var foundCell))
            {
                World.DefaultGameObjectInjectionWorld.EntityManager.AddComponentData(foundCell.Entity, new SpawnBubbleTagCmp());
                Destroy(gameObject);
            }

            transform.position += direction * Time.deltaTime * speed;

        }
    }
}
