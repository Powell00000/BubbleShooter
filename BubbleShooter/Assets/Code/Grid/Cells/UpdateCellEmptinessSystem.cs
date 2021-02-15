
using Assets.Code.Bubbles;
using Unity.Collections;
using Unity.Entities;

namespace Assets.Code.Grid.Cells
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    internal class UpdateCellEmptinessSystem : SystemBaseWithBarriers
    {
        private EntityQuery bubblesQuery;

        protected override void OnCreate()
        {
            base.OnCreate();
            bubblesQuery = GetEntityQuery(ComponentType.ReadOnly<BubbleCmp>());
        }
        protected override void OnUpdate()
        {
            var bubbles = bubblesQuery.ToEntityArray(Allocator.TempJob);

            Entities
                .WithDisposeOnCompletion(bubbles)
                .ForEach((Entity e, ref CellCmp cellCmp) =>
                {
                    if (!bubbles.Contains(cellCmp.OccupyingEntity))
                    {
                        cellCmp.OccupyingEntity = Entity.Null;
                    }
                })
                .Schedule();
        }
    }
}
