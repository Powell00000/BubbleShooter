using Assets.Code.Grid.Cells.Hybrid;
using Unity.Entities;

namespace Assets.Code.Grid.Cells
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    internal class MarkAsSelectedSystem : SystemBaseWithBarriers
    {
        protected override void OnUpdate()
        {
            var endSimBuffer = endSimulationBuffer.CreateCommandBuffer();

            Entities
                .WithoutBurst()
                .WithAll<MarkAsSelectedCmp>()
                .ForEach((Entity e, Cell cellMono) =>
                {
                    cellMono.gizmos = true;
                    endSimBuffer.RemoveComponent<MarkAsSelectedCmp>(e);
                })
                .Run();

            Entities
                .WithoutBurst()
                .WithNone<MarkAsSelectedCmp>()
                .ForEach((Entity e, Cell cellMono) =>
                {
                    cellMono.gizmos = false;
                })
                .Run();
        }
    }
}
