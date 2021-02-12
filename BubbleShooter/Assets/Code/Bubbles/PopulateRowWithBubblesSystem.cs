using Assets.Code.Grid.Row;
using Unity.Entities;

namespace Assets.Code.Bubbles
{
    internal class PopulateRowWithBubblesSystem : SystemBaseWithBarriers
    {
        protected override void OnUpdate()
        {
            var beginInitBuffer = beginInitializationBuffer.CreateCommandBuffer();
            var endSimBuffer = endSimulationBuffer.CreateCommandBuffer();

            Entities
                .WithoutBurst()
                .ForEach((Entity e, in PopulateRowWithBubbles populateRowCmp) =>
                {
                    MarkCellsToSpawnBubbles(populateRowCmp, beginInitBuffer);
                    endSimBuffer.DestroyEntity(e);
                })
                .Run();

            beginInitializationBuffer.AddJobHandleForProducer(Dependency);
            endSimulationBuffer.AddJobHandleForProducer(Dependency);
        }

        private void MarkCellsToSpawnBubbles(PopulateRowWithBubbles populateRowCmp, EntityCommandBuffer ecb)
        {
            Entities
                .WithSharedComponentFilter(new RowSharedCmp { RowNumber = populateRowCmp.Row })
                .ForEach((Entity e) =>
                {
                    ecb.AddComponent(e, new SpawnBubbleCmp() { RandomizeNumber = populateRowCmp.RandomizeNumbers });
                })
                .Schedule();
        }
    }
}
