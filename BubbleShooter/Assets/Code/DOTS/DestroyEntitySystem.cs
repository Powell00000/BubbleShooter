using Assets.Code.Visuals;
using Unity.Entities;

namespace Assets.Code.DOTS
{
    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
    //[DisableAutoCreation]
    internal class DestroyEntitySystem : SystemBaseWithBarriers
    {
        protected override void OnUpdate()
        {
            var endSimBuffer = endSimulationBuffer.CreateCommandBuffer();
            Entities
                .WithNone<IsAnimatingTagCmp>()
                .WithAll<DestroyTagCmp>()
                .ForEach((Entity e) =>
                {
                    endSimBuffer.DestroyEntity(e);
                })
                .Schedule();

            endSimulationBuffer.AddJobHandleForProducer(Dependency);
        }
    }
}
