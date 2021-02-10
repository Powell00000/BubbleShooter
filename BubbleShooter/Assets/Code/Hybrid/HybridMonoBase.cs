using Assets.Code.DOTS;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Assets.Code.Hybrid
{
    public abstract class HybridMonoBase : MonoBehaviour
    {
        protected Entity entity;

        public Entity Entity => entity;

        protected EntityManager entityManager => World.DefaultGameObjectInjectionWorld.EntityManager;

        protected void CreateEntity()
        {
            entity = entityManager.CreateEntity();
            entityManager.AddComponentData(entity, new LocalToWorld());
            entityManager.AddComponentData(entity, new Translation());
            entityManager.AddComponentData(entity, new JustSpawnedTagCmp());
            entityManager.AddComponentObject(Entity, this);
        }
    }
}