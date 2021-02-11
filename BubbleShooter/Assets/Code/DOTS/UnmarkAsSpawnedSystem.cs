
using Unity.Entities;

namespace Assets.Code.DOTS
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    internal class UnmarkAsSpawnedSystem : SystemBaseWithBarriers
    {
        protected override void OnUpdate()
        {
            var endSimBuffer = endSimulationBuffer.CreateCommandBuffer();
            Entities
                .WithAll<JustSpawnedTagCmp>()
                .ForEach((Entity e) =>
                {
                    endSimBuffer.RemoveComponent<JustSpawnedTagCmp>(e);
                })
                .Schedule();

            endSimulationBuffer.AddJobHandleForProducer(Dependency);
        }
    }
}
