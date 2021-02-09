using Assets.Code.Hybrid;
using Assets.Code.Movement.Follow;
using Unity.Entities;

namespace Assets.Code.Bubbles.Hybrid
{
    internal class Bubble : HybridMonoBase
    {
        public void CreateAndSetupBubbleEntity(Entity entityToFollow)
        {
            CreateEntity();
            entityManager.AddComponentData(entity, new BubbleCmp());
            entityManager.AddComponentData(entity, new FollowEntityCmp { EntityToFollow = entityToFollow });
        }
    }
}
