
using Assets.Code.DOTS;
using Assets.Code.Grid.Row;
using Assets.Code.Grid.Spawn;
using Unity.Entities;

namespace Assets.Code.Grid.Rows
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    internal class RowIndexUpdateSystem : SystemBaseWithBarriers
    {
        protected override void OnCreate()
        {
            base.OnCreate();
            RequireSingletonForUpdate<RowSpawnedTagCmp>();
        }

        protected override void OnUpdate()
        {
            var endSimBuffer = endSimulationBuffer.CreateCommandBuffer();
            Entities
                .WithoutBurst()
                .WithNone<JustSpawnedTagCmp>()
                .ForEach((Entity e, RowSharedCmp rowCmp) =>
                {
                    rowCmp.RowNumber++;
                    endSimBuffer.SetSharedComponent(e, rowCmp);
                })
                .Run();

            endSimulationBuffer.AddJobHandleForProducer(Dependency);
        }
    }
}
