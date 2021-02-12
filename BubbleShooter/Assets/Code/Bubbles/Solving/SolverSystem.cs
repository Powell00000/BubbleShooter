using Assets.Code.Bubbles.Hybrid;
using Assets.Code.DOTS;
using Assets.Code.Physics;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.Code.Bubbles.Solving
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    internal class SolverSystem : SystemBaseWithBarriers
    {
        private struct BubbleNode
        {
            public Bubble CurrentBubble;
            public Bubble[] Neighbours;
        }
        protected override void OnCreate()
        {
            base.OnCreate();
            RequireSingletonForUpdate<SolveHereTagCmp>();
        }

        protected override void OnUpdate()
        {
            Dictionary<Entity, BubbleNode> allBubblesWithSameNumber = new Dictionary<Entity, BubbleNode>();
            Entity solveHere = GetSingletonEntity<SolveHereTagCmp>();
            Bubble bubbleMono = EntityManager.GetComponentObject<Bubble>(solveHere);

            TraverseBubbles(ref allBubblesWithSameNumber, bubbleMono);

            Debug.Log($"Same numbers: {allBubblesWithSameNumber.Count}");

            if (allBubblesWithSameNumber.Count > 1)
            {
                int outputNumber = (int)math.pow(bubbleMono.Number, allBubblesWithSameNumber.Count);

                var beginInitBuffer = beginInitializationBuffer.CreateCommandBuffer();
                bool foundNextIteration = false;
                foreach (var node in allBubblesWithSameNumber.Values)
                {
                    for (int i = 0; i < node.Neighbours.Length; i++)
                    {
                        if (node.Neighbours[i].Number == outputNumber)
                        {
                            beginInitBuffer.SetComponent(node.CurrentBubble.Entity, new NumberCmp { Value = outputNumber });
                            beginInitBuffer.AddComponent(node.CurrentBubble.Entity, new SolveHereTagCmp());
                            foundNextIteration = true;
                            allBubblesWithSameNumber.Remove(node.CurrentBubble.Entity);
                            break;
                        }
                    }
                    if (foundNextIteration)
                    {
                        break;
                    }
                }
                if (!foundNextIteration)
                {
                    BubbleNode[] sortedNodesFromTopToBottom = new BubbleNode[allBubblesWithSameNumber.Values.Count];
                    allBubblesWithSameNumber.Values.CopyTo(sortedNodesFromTopToBottom, 0);
                    sortedNodesFromTopToBottom = sortedNodesFromTopToBottom.OrderBy((node) => -node.CurrentBubble.transform.position.y).ToArray();

                    foreach (var item in sortedNodesFromTopToBottom)
                    {
                        Debug.Log(item.CurrentBubble.transform.position);
                    }

                    BubbleNode mostTopNode = sortedNodesFromTopToBottom[0];
                    beginInitBuffer.SetComponent(mostTopNode.CurrentBubble.Entity, new NumberCmp { Value = outputNumber });
                    allBubblesWithSameNumber.Remove(mostTopNode.CurrentBubble.Entity);
                }

                foreach (var entity in allBubblesWithSameNumber.Keys)
                {
                    beginInitBuffer.AddComponent(entity, new DestroyTagCmp());
                }

                beginInitBuffer.CreateEntity(EntityManager.CreateArchetype(Archetypes.RefreshConnections));
                beginInitializationBuffer.AddJobHandleForProducer(Dependency);
            }

            var endSimBuffer = endSimulationBuffer.CreateCommandBuffer();
            endSimBuffer.RemoveComponent<SolveHereTagCmp>(solveHere);
            endSimulationBuffer.AddJobHandleForProducer(Dependency);
        }

        private void TraverseBubbles(ref Dictionary<Entity, BubbleNode> collectedBubbles, Bubble bubble)
        {
            BubbleNode node = new BubbleNode
            {
                CurrentBubble = bubble,
                Neighbours = PhysicsEx.GetNeighbouringBubbles(bubble.transform.position)
            };

            if (!collectedBubbles.ContainsKey(node.CurrentBubble.Entity))
            {
                collectedBubbles.Add(node.CurrentBubble.Entity, node);
            }

            foreach (var item in node.Neighbours)
            {
                if (!collectedBubbles.ContainsKey(item.Entity) && item.Number == node.CurrentBubble.Number)
                {
                    //collectedBubbles.Add(item.Entity, node);
                    TraverseBubbles(ref collectedBubbles, item);
                }
            }
        }
    }
}
