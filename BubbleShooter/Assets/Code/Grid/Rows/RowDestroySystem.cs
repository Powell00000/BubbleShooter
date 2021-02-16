using Assets.Code.DOTS;
using Assets.Code.Grid.Cells;
using Assets.Code.Grid.Row;
using Unity.Entities;

namespace Assets.Code.Grid.Rows
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    internal class RowDestroySystem : SystemBaseWithBarriers
    {
        protected override void OnUpdate()
        {
            var beginInitBuffer = beginInitializationBuffer.CreateCommandBuffer();
            int maxRowsCount = GameManager.MaxRowsCount;
            Entities
                .WithAll<CellCmp>()
                .WithNone<JustSpawnedTagCmp, DestroyTagCmp>()
                .ForEach((Entity e, ref RowSharedCmp rowCmp) =>
                {
                    if (rowCmp.RowNumber >= maxRowsCount)
                    {
                        beginInitBuffer.AddComponent(e, new DestroyTagCmp());
                    }
                })
                .Schedule();
            beginInitializationBuffer.AddJobHandleForProducer(Dependency);
        }
    }
}
