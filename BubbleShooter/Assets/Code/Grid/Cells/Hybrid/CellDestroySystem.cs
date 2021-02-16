using Assets.Code.DOTS;
using Unity.Entities;

namespace Assets.Code.Grid.Cells.Hybrid
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    internal class CellDestroySystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities
                .WithoutBurst()
                .WithAll<DestroyTagCmp>()
                .WithStructuralChanges()
                .ForEach((Entity e, Cell cellMono) =>
                {
                    cellMono.Destroy();
                })
                .Run();
        }
    }
}
