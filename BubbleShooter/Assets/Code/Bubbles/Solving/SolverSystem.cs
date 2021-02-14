using Assets.Code.Bubbles.Hybrid;
using Assets.Code.Bubbles.Nodes;
using Assets.Code.DOTS;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Assets.Code.Bubbles.Solving
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    internal class SolverSystem : SystemBaseWithBarriers
    {
        protected override void OnCreate()
        {
            base.OnCreate();
            RequireSingletonForUpdate<SolveHereTagCmp>();
        }

        protected override void OnUpdate()
        {
            Dictionary<Entity, DynamicBuffer<NodeNeighboursCmp>> bubblesWithSameNumber = new Dictionary<Entity, DynamicBuffer<NodeNeighboursCmp>>();

            Entity solveHere = GetSingletonEntity<SolveHereTagCmp>();
            Bubble bubbleMono = EntityManager.GetComponentObject<Bubble>(solveHere);

            TraverseBubbles(ref bubblesWithSameNumber, solveHere);

            Debug.Log($"Same numbers: {bubblesWithSameNumber.Count}");

            if (bubblesWithSameNumber.Count > 1)
            {
                int outputNumber = (int)math.pow(bubbleMono.Number, bubblesWithSameNumber.Count);

                var beginInitBuffer = beginInitializationBuffer.CreateCommandBuffer();
                bool foundNextIteration = false;
                foreach (var element in bubblesWithSameNumber)
                {
                    Entity currentEntity = element.Key;
                    var neighboursBuffer = element.Value;
                    for (int i = 0; i < neighboursBuffer.Length; i++)
                    {
                        NumberCmp number = EntityManager.GetComponentData<NumberCmp>(neighboursBuffer[i].Neighbour);

                        if (number.Value == outputNumber)
                        {
                            beginInitBuffer.SetComponent(currentEntity, new NumberCmp { Value = outputNumber });
                            beginInitBuffer.AddComponent(currentEntity, new SolveHereTagCmp());
                            foundNextIteration = true;
                            bubblesWithSameNumber.Remove(currentEntity);
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
                    var sortedEntities = bubblesWithSameNumber.Keys.OrderBy((node) => -EntityManager.GetComponentData<Translation>(node).Value.y).ToList();

                    Entity topMostEntity = sortedEntities[0];
                    beginInitBuffer.SetComponent(topMostEntity, new NumberCmp { Value = outputNumber });
                    bubblesWithSameNumber.Remove(topMostEntity);
                }

                foreach (var entity in bubblesWithSameNumber.Keys)
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

        private void TraverseBubbles(ref Dictionary<Entity, DynamicBuffer<NodeNeighboursCmp>> collectedBubbles, Entity currrentEntity)
        {
            var neighbours = EntityManager.GetBuffer<NodeNeighboursCmp>(currrentEntity);
            if (!collectedBubbles.ContainsKey(currrentEntity))
            {
                collectedBubbles.Add(currrentEntity, neighbours);
            }

            foreach (var nodeNeighbour in neighbours)
            {
                if (!collectedBubbles.ContainsKey(nodeNeighbour.Neighbour))
                {
                    NumberCmp neighbourNumber = EntityManager.GetComponentData<NumberCmp>(nodeNeighbour.Neighbour);
                    NumberCmp bubbleNumber = EntityManager.GetComponentData<NumberCmp>(currrentEntity);

                    if (neighbourNumber.Value == bubbleNumber.Value)
                    {
                        TraverseBubbles(ref collectedBubbles, nodeNeighbour.Neighbour);
                    }
                }
            }
        }
    }
}
