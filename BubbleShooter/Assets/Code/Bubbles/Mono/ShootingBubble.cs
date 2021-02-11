using Unity.Entities;
using UnityEngine;

namespace Assets.Code.Bubbles.Mono
{
    internal class ShootingBubble : MonoBehaviour
    {
        [SerializeField]
        SphereCollider sphereCollider;

        private Vector3 direction;
        private float speed = 10;

        public void SetDirection(Vector3 direction)
        {
            this.direction = direction;
        }

        private void Update()
        {
            //sphereCollider.radius = transform.localScale.x / 2;
        }

        private void FixedUpdate()
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
