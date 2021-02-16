using Assets.Code.Bubbles.Connections;
using Assets.Code.DOTS;
using Assets.Code.Mono;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Assets.Code.Bubbles.Hybrid
{
    internal class CheckBottomBoundsForDroppedBubbleSystem : SystemBaseWithBarriers
    {
        [Zenject.Inject]
        private CameraBounds cameraBounds;

        protected override void OnUpdate()
        {
            float3 bottomBounds = cameraBounds.Bottom;
            float cellDiameter = GameManager.CellDiameter;

            var beginInitBuffer = beginInitializationBuffer.CreateCommandBuffer();

            Entities
                .WithAll<BubbleIsDroppingTagCmp>()
                .WithNone<DestroyTagCmp, UnderBottomBoundsTagCmp>()
                .ForEach((Entity e, in LocalToWorld localToWorld) =>
                {
                    if (localToWorld.Position.y < bottomBounds.y - cellDiameter)
                    {
                        beginInitBuffer.AddComponent(e, new UnderBottomBoundsTagCmp());
                    }
                })
                .Schedule();

            beginInitializationBuffer.AddJobHandleForProducer(Dependency);
        }
    }
}