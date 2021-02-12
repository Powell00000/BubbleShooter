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
            List<Entity> bubblesWithSameNumber = new List<Entity>();
            Entity solveHere = GetSingletonEntity<SolveHereTagCmp>();
            NumberCmp numberCmp = EntityManager.GetComponentData<NumberCmp>(solveHere);

            bubblesWithSameNumber.AddRange(GetBubblesWithSameNumberNearEntity(solveHere, numberCmp.Value));

            Debug.Log($"Same numbers at start: {bubblesWithSameNumber.Count}");

            for (int i = 0; i < bubblesWithSameNumber.Count; i++)
            {
                bubblesWithSameNumber.AddRange(GetBubblesWithSameNumberNearEntity(bubblesWithSameNumber[i], numberCmp.Value));
            }

            Debug.Log($"Same numbers at end: {bubblesWithSameNumber.Count}");

            var endSimBuffer = endSimulationBuffer.CreateCommandBuffer();
            endSimBuffer.RemoveComponent<SolveHereTagCmp>(solveHere);
            endSimulationBuffer.AddJobHandleForProducer(Dependency);
        }

        private List<Entity> GetBubblesWithSameNumberNearEntity(Entity entity, int number)
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

            return bubblesWithSameNumber;
        }
    }
}
