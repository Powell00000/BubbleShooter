using Unity.Entities;

namespace Assets.Code.DOTS
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    internal class DestroyEntitySystem : SystemBaseWithBarriers
    {
        protected override void OnUpdate()
        {
            var endSimBuffer = endSimulationBuffer.CreateCommandBuffer();
            Entities
                .WithAll<DestroyTagCmp>()
                .ForEach((Entity e) =>
                {
                    endSimBuffer.DestroyEntity(e);
                })
                .Run();

            endSimulationBuffer.AddJobHandleForProducer(Dependency);
        }
    }
}
