using Assets.Code.Bubbles.Solving;
using Assets.Code.DOTS;
using Assets.Code.Grid.Cells;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Assets.Code.Bubbles.Hybrid
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    internal class BubbleSpawnSystem : SystemBaseWithBarriers
    {
        [Zenject.Inject]
        private Bubble bubblePrefab;

        [Zenject.Inject]
        private GameManager gameManager;

        protected override void OnUpdate()
        {
            var endSimBuffer = endSimulationBuffer.CreateCommandBuffer();

            Entities
                .WithoutBurst()
                .WithStructuralChanges()
                .ForEach((Entity e, ref CellCmp cellCmp, in Scale scaleCmp, in SpawnBubbleCmp spawnBubbleCmp, in Translation translation) =>
                {
                    var bubble = UnityEngine.Object.Instantiate(bubblePrefab, translation.Value, quaternion.identity).GetComponent<Bubble>();
                    bubble.CreateAndSetupBubbleEntity(e, scaleCmp);

                    if (spawnBubbleCmp.SolveHere)
                    {
                        endSimBuffer.AddComponent(bubble.Entity, new SolveHereTagCmp());
                        Debug.Log("spawn");
                    }

                    if (spawnBubbleCmp.RandomizeNumber)
                    {
                        EntityManager.SetComponentData(bubble.Entity, new NumberCmp { Value = gameManager.GetRandomBubbleNumber() });
                    }
                    else
                    {
                        EntityManager.SetComponentData(bubble.Entity, new NumberCmp { Value = spawnBubbleCmp.Number });
                    }

                    cellCmp.OccupyingEntity = bubble.Entity;

                    EntityManager.RemoveComponent<SpawnBubbleCmp>(e);
                })
                .Run();

            EntityManager.CreateEntity(Archetypes.RefreshConnections);
        }
    }
}
