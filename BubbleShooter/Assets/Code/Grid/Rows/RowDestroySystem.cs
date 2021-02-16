
using Assets.Code.DOTS;
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
            Entities
                .WithoutBurst()
                .WithNone<JustSpawnedTagCmp, DestroyTagCmp>()
                .ForEach((Entity e, RowSharedCmp rowCmp) =>
                {
                    if (rowCmp.RowNumber >= GameManager.MaxRowsCount)
                    {
                        beginInitBuffer.AddComponent(e, new DestroyTagCmp());
                    }
                    beginInitBuffer.SetSharedComponent(e, rowCmp);
                })
                .Run();

            beginInitializationBuffer.AddJobHandleForProducer(Dependency);
        }
    }
}
