using Assets.Code.DOTS;
using System.Collections.Generic;
using Unity.Entities;
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
            HashSet<Entity> allBubblesWithSameNumber = new HashSet<Entity>();
            Entity solveHere = GetSingletonEntity<SolveHereTagCmp>();
            NumberCmp numberCmp = EntityManager.GetComponentData<NumberCmp>(solveHere);

            allBubblesWithSameNumber.Add(solveHere);

            TraverseBubbles(ref allBubblesWithSameNumber, solveHere, ref numberCmp);

            Debug.Log($"Same numbers: {allBubblesWithSameNumber.Count}");

            if (allBubblesWithSameNumber.Count > 1)
            {
                var beginInitBuffer = beginInitializationBuffer.CreateCommandBuffer();
                foreach (var bubble in allBubblesWithSameNumber)
                {
                    beginInitBuffer.AddComponent(bubble, new DestroyTagCmp());
                }
                beginInitializationBuffer.AddJobHandleForProducer(Dependency);
            }

            var endSimBuffer = endSimulationBuffer.CreateCommandBuffer();
            endSimBuffer.RemoveComponent<SolveHereTagCmp>(solveHere);
            endSimulationBuffer.AddJobHandleForProducer(Dependency);
        }

        private void TraverseBubbles(ref HashSet<Entity> collectedBubbles, Entity bubble, ref NumberCmp number)
        {
            var bubblesNearEntity = GetBubblesWithSameNumberNearEntity(bubble, number.Value);
            foreach (var item in bubblesNearEntity)
            {
                if (!collectedBubbles.Contains(item))
                {
                    collectedBubbles.Add(item);
                    TraverseBubbles(ref collectedBubbles, item, ref number);
                }
            }
        }

        private Entity[] GetBubblesWithSameNumberNearEntity(Entity entity, int number)
        {
            List<Entity> bubblesWithSameNumber = new List<Entity>(6);

            Translation translation = EntityManager.GetComponentData<Translation>(entity);
            var nearBubbles = Physics.PhysicsEx.GetNeighbouringBubbles(translation.Value);

            for (int i = 0; i < nearBubbles.Length; i++)
            {
                if (nearBubbles[i].Number == number)
                {
                    bubblesWithSameNumber.Add(nearBubbles[i].Entity);
                }
            }

            return bubblesWithSameNumber.ToArray();
        }
    }
}
