using Unity.Entities;

namespace Assets.Code.Bubbles.Hybrid
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    internal class RefreshNumberSystem : SystemBaseWithBarriers
    {
        protected override void OnUpdate()
        {
            var beginInitBuffer = beginInitializationBuffer.CreateCommandBuffer();
            Entities
                .WithoutBurst()
                .ForEach((Entity e, Bubble bubbleMono, in NumberCmp numberCmp) =>
                {
                    bubbleMono.RefreshNumber(numberCmp.Value, beginInitBuffer);
                })
                .Run();
            beginInitializationBuffer.AddJobHandleForProducer(Dependency);
        }
    }
}
