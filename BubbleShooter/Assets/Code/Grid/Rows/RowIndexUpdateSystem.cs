
using Assets.Code.DOTS;
using Assets.Code.Grid.Row;
using Assets.Code.Grid.Spawn;
using Unity.Entities;
using UnityEngine;

namespace Assets.Code.Grid.Rows
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    internal class RowIndexUpdateSystem : SystemBaseWithBarriers
    {
        protected override void OnCreate()
        {
            base.OnCreate();
            //RequireSingletonForUpdate<RowSpawnedTagCmp>();
        }

        protected override void OnUpdate()
        {
            if (HasSingleton<RowSpawnedTagCmp>())
            {
                Debug.Log("rowSpawned");
            }
            else
            {
                return;
            }

            var beginInitBuffer = beginInitializationBuffer.CreateCommandBuffer();
            var endSimBuffer = endSimulationBuffer.CreateCommandBuffer();
            Entities
                .WithoutBurst()
                .WithNone<JustSpawnedTagCmp>()
                .ForEach((Entity e, RowSharedCmp rowCmp) =>
                {
                    rowCmp.RowNumber++;
                    if (rowCmp.RowNumber >= rowCmp.MaxRowNumber)
                    {
                        beginInitBuffer.AddComponent(e, new DestroyTagCmp());
                    }
                    endSimBuffer.SetSharedComponent(e, rowCmp);
                })
                .Run();

            beginInitializationBuffer.AddJobHandleForProducer(Dependency);
            endSimulationBuffer.AddJobHandleForProducer(Dependency);
        }
    }
}
