using Assets.Code.DOTS;
using Assets.Code.Grid.Cells;
using Assets.Code.Grid.Cells.Hybrid;
using Assets.Code.Grid.Row;
using Assets.Code.Physics;
using System.Collections.Generic;
using Unity.Entities;

namespace Assets.Code.Bubbles.Connections
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [DisableAutoCreation]
    internal class HasConnectionWithTopRowSystem : SystemBaseWithBarriers
    {
        private struct CellNode
        {
            public Cell CurrentCell;
            public Cell[] Neighbours;
        }

        protected override void OnUpdate()
        {
            Dictionary<Entity, CellNode> collectedCells = new Dictionary<Entity, CellNode>();

            var beginInitBuffer = beginInitializationBuffer.CreateCommandBuffer();

            Entities
                .WithoutBurst()
                .WithNone<HasConnectionWithTopRowTagCmp>()
                .WithSharedComponentFilter(new RowSharedCmp { RowNumber = 1 })
                .ForEach((Entity e, Cell cellMono) =>
                {
                    TraverseOccupiedCells(ref collectedCells, cellMono);
                })
                .Run();

            if (collectedCells.Count > 0)
            {
                foreach (var item in collectedCells.Keys)
                {
                    beginInitBuffer.AddComponent(item, new HasConnectionWithTopRowTagCmp());
                }
                beginInitBuffer.CreateEntity(EntityManager.CreateArchetype(Archetypes.ConnectionsRefreshed));
                beginInitializationBuffer.AddJobHandleForProducer(Dependency);
            }
        }

        private void TraverseOccupiedCells(ref Dictionary<Entity, CellNode> collectedCells, Cell cell)
        {
            CellNode node = new CellNode
            {
                CurrentCell = cell,
                Neighbours = PhysicsEx.GetNeighbouringCells(cell.transform.position)
            };

            if (!collectedCells.ContainsKey(node.CurrentCell.Entity))
            {
                collectedCells.Add(node.CurrentCell.Entity, node);
            }

            foreach (var item in node.Neighbours)
            {
                CellCmp cellCmp = EntityManager.GetComponentData<CellCmp>(item.Entity);
                if (!collectedCells.ContainsKey(item.Entity) && !cellCmp.IsEmpty)
                {
                    TraverseOccupiedCells(ref collectedCells, item);
                }
            }
        }
    }
}
