using Assets.Code.Hybrid;
using Assets.Code.Movement.Follow;
using Unity.Entities;
using Unity.Transforms;

namespace Assets.Code.Bubbles.Hybrid
{
    internal class Bubble : HybridMonoBase
    {
        public void CreateAndSetupBubbleEntity(Entity entityToFollow)
        {
            CreateEntity();
            entityManager.AddComponentData(entity, new BubbleCmp());
            entityManager.AddComponentData(entity, new FollowEntityCmp { EntityToFollow = entityToFollow });
            entityManager.AddComponentData(entity, new CopyTransformToGameObject());
            entityManager.AddComponentObject(entity, transform);

            entityManager.SetComponentData(entity, entityManager.GetComponentData<Translation>(entityToFollow));
        }
    }
}
