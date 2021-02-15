using Assets.Code.DOTS;
using Assets.Code.Grid.Cells;
using Unity.Entities;

namespace Assets.Code.Bubbles.Connections
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    internal class MarkDisconnectedBubblesSystem : SystemBaseWithBarriers
    {
        protected override void OnCreate()
        {
            base.OnCreate();
            RequireSingletonForUpdate<ConnectionsRefreshedTagCmp>();
        }

        protected override void OnUpdate()
        {
            var beginSimBuffer = beginSimulationBuffer.CreateCommandBuffer();
            var endSimBuffer = endSimulationBuffer.CreateCommandBuffer();
            bool bubblesRemoved = false;
            Entities
                .WithNone<HasConnectionWithTopRowTagCmp>()
                .ForEach((Entity e, ref CellCmp cellCmp) =>
                {
                    if (!cellCmp.IsEmpty)
                    {
                        bubblesRemoved = true;
                        beginSimBuffer.AddComponent(cellCmp.OccupyingEntity, new DisconnectedBubbleTagCmp());
                    }
                })
                .Run();

            endSimBuffer.DestroyEntity(GetSingletonEntity<ConnectionsRefreshedTagCmp>());
            if (bubblesRemoved)
            {
                beginSimBuffer.CreateEntity(EntityManager.CreateArchetype(Archetypes.RefreshConnections));
            }

            beginSimulationBuffer.AddJobHandleForProducer(Dependency);
            endSimulationBuffer.AddJobHandleForProducer(Dependency);
        }
    }
}
