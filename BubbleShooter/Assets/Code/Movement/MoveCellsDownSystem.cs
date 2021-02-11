using Assets.Code.Grid.Cells;
using Assets.Code.Grid.Spawn;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Assets.Code.Movement
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    internal class MoveCellsDownSystem : SystemBaseWithBarriers
    {
        protected override void OnUpdate()
        {
            var beginSimBuffer = beginSimulationBuffer.CreateCommandBuffer();
            var endSimBuffer = endSimulationBuffer.CreateCommandBuffer();

            //add MoveDownCmp to entities
            if (HasSingleton<RowSpawnedTagCmp>())
            {
                Entities
                    .WithAll<CellCmp>()
                    .WithNone<MoveDownCmp>()
                    .ForEach((Entity e) =>
                    {
                        beginSimBuffer.AddComponent(e, new MoveDownCmp { TimeLeft = 0.2f });
                    })
                    .Schedule();

                beginSimulationBuffer.AddJobHandleForProducer(Dependency);
                endSimBuffer.DestroyEntity(GetSingletonEntity<RowSpawnedTagCmp>());
            }


            Entities
                .ForEach((Entity e, ref MoveDownCmp moveDownCmp, ref Translation translation, in CellCmp cellCmp) =>
                {
                    translation.Value -= new float3(0, cellCmp.Diameter, 0);
                    endSimBuffer.RemoveComponent<MoveDownCmp>(e);
                })
                .Schedule();


            endSimulationBuffer.AddJobHandleForProducer(Dependency);
        }
    }
}
