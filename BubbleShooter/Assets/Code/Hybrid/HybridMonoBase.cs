using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Assets.Code.Hybrid
{
    public abstract class HybridMonoBase : MonoBehaviour
    {
        protected Entity entity;

        public Entity Entity => entity;

        protected EntityManager entityManager => World.DefaultGameObjectInjectionWorld.EntityManager;

        public virtual void UpdatePosition()
        {
            transform.position = entityManager.GetComponentData<Translation>(entity).Value;
        }

        public virtual void SetPosition(float3 newPosition)
        {
            transform.position = newPosition;
        }

        protected void CreateEntity()
        {
            entity = entityManager.CreateEntity();
            entityManager.AddComponentData(entity, new LocalToWorld());
            entityManager.AddComponentData(entity, new Translation());
            entityManager.AddComponentObject(Entity, this);
        }
    }
}