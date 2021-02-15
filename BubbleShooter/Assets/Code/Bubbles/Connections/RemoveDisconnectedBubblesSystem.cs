
using Assets.Code.DOTS;
using Unity.Entities;

namespace Assets.Code.Bubbles.Connections
{
    internal class RemoveDisconnectedBubblesSystem : SystemBaseWithBarriers
    {
        protected override void OnUpdate()
        {
            var beginInitBuffer = beginInitializationBuffer.CreateCommandBuffer();
            Entities
                .WithAll<DisconnectedBubbleTagCmp>()
                .WithNone<DestroyTagCmp>()
                .ForEach((Entity e) =>
                {
                    beginInitBuffer.AddComponent(e, new DestroyTagCmp());
                })
                .Schedule();
            beginInitializationBuffer.AddJobHandleForProducer(Dependency);
        }
    }
}
