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
                    .WithNone<MoveDownTagCmp>()
                    .ForEach((Entity e) =>
                    {
                        beginSimBuffer.AddComponent(e, new MoveDownTagCmp { });
                    })
                    .Schedule();

                beginSimulationBuffer.AddJobHandleForProducer(Dependency);
                endSimBuffer.DestroyEntity(GetSingletonEntity<RowSpawnedTagCmp>());
            }


            Entities
                .ForEach((Entity e, ref MoveDownTagCmp moveDownCmp, ref Translation translation, in CellCmp cellCmp) =>
                {
                    translation.Value -= new float3(0, cellCmp.Diameter, 0);
                    endSimBuffer.RemoveComponent<MoveDownTagCmp>(e);
                })
                .Schedule();


            endSimulationBuffer.AddJobHandleForProducer(Dependency);
        }
    }
}
