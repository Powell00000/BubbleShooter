using Assets.Code.Bubbles.Explosion;
using Assets.Code.DOTS;
using Unity.Entities;

namespace Assets.Code.Bubbles.Connections
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    internal class MarkDisconnectedBubblesSystem : SystemBaseWithBarriers
    {
        protected override void OnCreate()
        {
            base.OnCreate();
        }

        protected override void OnUpdate()
        {
            var beginSimBuffer = beginSimulationBuffer.CreateCommandBuffer();
            var endSimBuffer = endSimulationBuffer.CreateCommandBuffer();
            Entities
                .WithNone<HasConnectionWithTopRowTagCmp, DisconnectedBubbleTagCmp, DestroyTagCmp>()
                .WithNone<ExplodeTagCmp>()
                .ForEach((Entity e, ref BubbleCmp bubbleCmp) =>
                {
                    beginSimBuffer.AddComponent(e, new DisconnectedBubbleTagCmp());
                })
                .Schedule();

            beginSimulationBuffer.AddJobHandleForProducer(Dependency);
            endSimulationBuffer.AddJobHandleForProducer(Dependency);
        }
    }
}
