using Assets.Code.DOTS;
using Unity.Entities;
using UnityEngine;
using static Assets.Code.Physics.PhysicsEx;

namespace Assets.Code.Bubbles.Mono
{
    internal class ShootingBubble : MonoBehaviour
    {
        [SerializeField]
        private SphereCollider sphereCollider;

        [SerializeField]
        private TMPro.TMP_Text numberText;

        private Vector3 direction;
        private float speed = 10;
        private Entity bubbleIsShootingTagEntity;
        private int number;

        private void Awake()
        {
            bubbleIsShootingTagEntity = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntity(Archetypes.BubbleIsShooting);
        }

        public void SetNumber(int number)
        {
            this.number = number;
            numberText.text = number.ToString();
        }

        public void SetDirection(Vector3 direction)
        {
            this.direction = direction;
        }

        private void Update()
        {
            float extrapolatedDistance = (direction * Time.deltaTime * speed).magnitude;
            Vector3 extrapolatedPosition = transform.position + direction * extrapolatedDistance;

            CastResult castResult = Cast(direction, extrapolatedDistance, transform.position);

            if (castResult.SomethingHit)
            {
                direction = castResult.ReflectedDirection;

                if (castResult.FoundCell != null)
                {
                    World.DefaultGameObjectInjectionWorld.EntityManager.AddComponentData(castResult.FoundCell.Entity, new SpawnBubbleCmp() { SolveHere = true, RandomizeNumber = false, Number = number });
                    World.DefaultGameObjectInjectionWorld.EntityManager.AddComponentData(bubbleIsShootingTagEntity, new DestroyTagCmp());

                    Destroy(gameObject);
                }


                if (extrapolatedDistance > Vector3.Distance(transform.position, castResult.ContactPoint))
                {
                    transform.position = castResult.ContactPoint;
                }
                else
                {
                    transform.position = extrapolatedPosition;
                }
            }
            else
            {
                transform.position = extrapolatedPosition;
            }
        }
    }
}
