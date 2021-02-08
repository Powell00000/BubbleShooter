using Assets.Code.Grid;
using Assets.Code.Spawn;
using Unity.Entities;
//using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Assets.Code.Movement
{
    internal class MoveCellsDownSystem : SystemBaseWithBarriers
    {
        protected override void OnCreate()
        {
            base.OnCreate();
        }

        protected override void OnUpdate()
        {
            var beginSimBuffer = beginSimulationBuffer.CreateCommandBuffer().AsParallelWriter();
            //add MoveDownCmp to entities

            if (HasSingleton<RowSpawnedTagCmp>())
            {
                Entities
                    .WithAll<CellCmp>()
                    .WithNone<MoveDownCmp>()
                    .ForEach((Entity e, int entityInQueryIndex) =>
                    {
                        beginSimBuffer.AddComponent(entityInQueryIndex, e, new MoveDownCmp { TimeLeft = 1 });
                    })
                    .Schedule();

                beginSimulationBuffer.AddJobHandleForProducer(Dependency);
                EntityManager.DestroyEntity(GetSingletonEntity<RowSpawnedTagCmp>());
            }

            var endSimBuffer = endSimulationBuffer.CreateCommandBuffer().AsParallelWriter();

            //update MoveDownCmp
            float dt = Time.DeltaTime;
            Entities
                .WithAll<CellCmp>()
                .ForEach((Entity e, int entityInQueryIndex, ref MoveDownCmp moveDownCmp, ref Translation translation) =>
                {
                    if (moveDownCmp.TimeLeft > 0)
                    {
                        moveDownCmp.TimeLeft -= dt;
                        translation.Value -= new float3(0, 1, 0) * dt;
                    }
                    else
                    {
                        endSimBuffer.RemoveComponent<MoveDownCmp>(entityInQueryIndex, e);
                    }
                })
                .Schedule();


            endSimulationBuffer.AddJobHandleForProducer(Dependency);
        }
    }
}
