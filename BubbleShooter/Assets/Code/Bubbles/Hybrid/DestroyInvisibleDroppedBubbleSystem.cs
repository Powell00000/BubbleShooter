using Assets.Code.DOTS;
using Unity.Entities;

namespace Assets.Code.Bubbles.Hybrid
{
    internal class DestroyInvisibleDroppedBubbleSystem : SystemBaseWithBarriers
    {
        protected override void OnUpdate()
        {

            var beginInitBuffer = beginInitializationBuffer.CreateCommandBuffer();

            Entities
                .WithoutBurst()
                .WithStructuralChanges()
                .WithAll<UnderBottomBoundsTagCmp>()
                .WithNone<DestroyTagCmp>()
                .ForEach((Entity e, Bubble bubble) =>
                {
                    bubble.StopAllVisuals();
                    beginInitBuffer.AddComponent<DestroyTagCmp>(e);
                })
                .Run();

            beginInitializationBuffer.AddJobHandleForProducer(Dependency);
        }
    }
}