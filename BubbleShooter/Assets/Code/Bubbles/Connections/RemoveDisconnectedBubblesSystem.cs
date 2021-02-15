
using Assets.Code.DOTS;
using Unity.Entities;

namespace Assets.Code.Bubbles.Connections
{
    internal class RemoveDisconnectedBubblesSystem : SystemBaseWithBarriers
    {
        protected override void OnUpdate()
        {
            var endSimBuffer = endSimulationBuffer.CreateCommandBuffer();
            Entities
                .WithAll<DisconnectedBubbleTagCmp>()
                .WithNone<DestroyTagCmp>()
                .ForEach((Entity e) =>
                {
                    endSimBuffer.AddComponent(e, new DestroyTagCmp());
                })
                .Schedule();
            endSimulationBuffer.AddJobHandleForProducer(Dependency);
        }
    }
}
