using Assets.Code.Bubbles.Solving;
using Assets.Code.Grid.Cells;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

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
            var beginSimBuffer = beginSimulationBuffer.CreateCommandBuffer();
            Entities
                .WithoutBurst()
                .WithStructuralChanges()
                .WithAll<CellCmp>()
                .ForEach((Entity e, in Scale scaleCmp, in SpawnBubbleCmp spawnBubbleCmp) =>
                {
                    var bubble = UnityEngine.Object.Instantiate(bubblePrefab).GetComponent<Bubble>();
                    bubble.CreateAndSetupBubbleEntity(e, scaleCmp);

                    if (spawnBubbleCmp.SolveHere)
                    {
                        beginSimBuffer.AddComponent(bubble.Entity, new SolveHereTagCmp());
                    }

                    if (spawnBubbleCmp.RandomizeNumber)
                    {
                        beginSimBuffer.SetComponent(bubble.Entity, new NumberCmp { Value = gameManager.GetRandomBubbleNumber() });
                    }
                    else
                    {
                        beginSimBuffer.SetComponent(bubble.Entity, new NumberCmp { Value = spawnBubbleCmp.Number });
                    }

                    beginSimBuffer.RemoveComponent<SpawnBubbleCmp>(e);
                })
                .Run();

            beginSimulationBuffer.AddJobHandleForProducer(Dependency);
        }
    }
}
