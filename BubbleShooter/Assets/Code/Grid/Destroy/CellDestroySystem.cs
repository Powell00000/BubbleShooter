using Assets.Code.DOTS;
using Assets.Code.Grid.Cells.Hybrid;
using Unity.Entities;

namespace Assets.Code.Grid.Destroy
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    internal class CellDestroySystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities
                .WithoutBurst()
                .WithAll<DestroyTagCmp>()
                .ForEach((Entity e, Cell cellMono) =>
                {
                    cellMono.Destroy();
                })
                .Run();
        }
    }
}
