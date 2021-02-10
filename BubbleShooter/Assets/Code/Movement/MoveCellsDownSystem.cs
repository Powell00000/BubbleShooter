using Assets.Code.Grid;
using Assets.Code.Grid.Spawn;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Assets.Code.Movement
{
    internal class MoveCellsDownSystem : SystemBaseWithBarriers
    {
        protected override void OnUpdate()
        {
            var beginSimBuffer = beginSimulationBuffer.CreateCommandBuffer();

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
                EntityManager.DestroyEntity(GetSingletonEntity<RowSpawnedTagCmp>());
            }

            var endSimBuffer = endSimulationBuffer.CreateCommandBuffer();

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
