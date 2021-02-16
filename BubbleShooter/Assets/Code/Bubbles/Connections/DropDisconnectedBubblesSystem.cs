
using Assets.Code.Bubbles.Hybrid;
using Assets.Code.DOTS;
using Unity.Entities;

namespace Assets.Code.Bubbles.Connections
{
    internal class DropDisconnectedBubblesSystem : SystemBaseWithBarriers
    {
        protected override void OnUpdate()
        {
            //var beginInitBuffer = beginInitializationBuffer.CreateCommandBuffer();
            Entities
                .WithAll<DisconnectedBubbleTagCmp>()
                .WithNone<DestroyTagCmp, BubbleIsDroppingTagCmp>()
                .WithoutBurst()
                .WithStructuralChanges()
                .ForEach((Entity e, Bubble bubbleMono) =>
                {
                    //beginInitBuffer.AddComponent(e, new DestroyTagCmp());
                    bubbleMono.DropBubble();
                })
                //.Schedule();
                .Run();
            //beginInitializationBuffer.AddJobHandleForProducer(Dependency);
        }
    }
}
