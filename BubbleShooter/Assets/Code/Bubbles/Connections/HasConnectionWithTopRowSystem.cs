using Assets.Code.Bubbles.Explosion;
using Assets.Code.Bubbles.Nodes;
using Assets.Code.DOTS;
using Assets.Code.Grid.Cells.Hybrid;
using Assets.Code.Grid.Row;
using System.Collections.Generic;
using Unity.Entities;

namespace Assets.Code.Bubbles.Connections
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    [UpdateAfter(typeof(BubbleNodeUpdateSystem))]
    internal class HasConnectionWithTopRowSystem : SystemBaseWithBarriers
    {
        private struct CellNode
        {
            public Cell CurrentCell;
            public Cell[] Neighbours;
        }

        protected override void OnCreate()
        {
            base.OnCreate();
        }

        protected override void OnUpdate()
        {
            HashSet<Entity> collectedCells = new HashSet<Entity>();

            var beginSimBuffer = beginSimulationBuffer.CreateCommandBuffer();

            Entities
                .WithoutBurst()
                .WithNone<HasConnectionWithTopRowTagCmp>()
                .WithSharedComponentFilter(new RowSharedCmp { RowNumber = 1 })
                .WithAll<BubbleCmp, NodeNeighboursCmp>()
                .WithNone<DestroyTagCmp>()
                .ForEach((Entity e) =>
                {
                    TraverseOccupiedCells(ref collectedCells, e);
                })
                .Run();

            if (collectedCells.Count > 0)
            {
                foreach (var entity in collectedCells)
                {
                    beginSimBuffer.AddComponent(entity, new HasConnectionWithTopRowTagCmp());
                }
                beginSimulationBuffer.AddJobHandleForProducer(Dependency);
            }
        }

        private void TraverseOccupiedCells(ref HashSet<Entity> collectedBubbles, Entity thisEntity)
        {
            if (!collectedBubbles.Contains(thisEntity))
            {
                collectedBubbles.Add(thisEntity);
            }
            DynamicBuffer<NodeNeighboursCmp> neighbours = EntityManager.GetBuffer<NodeNeighboursCmp>(thisEntity);
            foreach (var item in neighbours)
            {
                if (!collectedBubbles.Contains(item.Neighbour) && !EntityManager.HasComponent<DestroyTagCmp>(item.Neighbour))
                {
                    TraverseOccupiedCells(ref collectedBubbles, item.Neighbour);
                }
            }
        }
    }
}
