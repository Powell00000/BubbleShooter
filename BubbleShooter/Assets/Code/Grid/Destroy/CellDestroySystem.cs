using Assets.Code.DOTS;
using Assets.Code.Grid.Cells.Hybrid;
using Unity.Entities;

namespace Assets.Code.Grid.Destroy
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    internal class CellDestroySystem : SystemBaseWithBarriers
    {
        protected override void OnUpdate()
        {
            var endSimBuffer = endSimulationBuffer.CreateCommandBuffer();
            Entities
                .WithoutBurst()
                .WithAll<DestroyTagCmp>()
                .ForEach((Entity e, Cell cellMono) =>
                {
                    cellMono.Destroy();
                    endSimBuffer.DestroyEntity(e);
                })
                .Run();

            endSimulationBuffer.AddJobHandleForProducer(Dependency);
        }
    }
}
