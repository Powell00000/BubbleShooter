using Assets.Code.Grid;
using Assets.Code.Grid.Spawn;
using Unity.Entities;
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

            //update MoveDownCmp
            float dt = Time.DeltaTime;
            Entities
                .ForEach((Entity e, ref MoveDownCmp moveDownCmp, ref Translation translation, in CellCmp cellCmp) =>
                {
                    if (moveDownCmp.TimeLeft > 0)
                    {
                        moveDownCmp.TimeLeft -= dt;
                        translation.Value -= new float3(0, cellCmp.Diameter, 0) * dt;
                    }
                    else
                    {
                        endSimBuffer.RemoveComponent<MoveDownCmp>(e);
                    }
                })
                .Schedule();


            endSimulationBuffer.AddJobHandleForProducer(Dependency);
        }
    }
}
