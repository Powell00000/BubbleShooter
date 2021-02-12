using Assets.Code.Bubbles.Solving;
using Assets.Code.Grid.Cells;
using System;
using Unity.Entities;
using Unity.Transforms;

namespace Assets.Code.Bubbles.Hybrid
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    internal class BubbleSpawnSystem : SystemBaseWithBarriers
    {
        [Zenject.Inject]
        private Bubble bubblePrefab;

        private Random random;

        protected override void OnCreate()
        {
            base.OnCreate();
            random = new Random(2);
        }

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
                        beginSimBuffer.SetComponent(bubble.Entity, new NumberCmp { Value = (int)math.pow(2, random.NextInt(1, 4)) });
                    }

                    beginSimBuffer.RemoveComponent<SpawnBubbleCmp>(e);
                })
                .Run();

            beginSimulationBuffer.AddJobHandleForProducer(Dependency);
        }
    }
}
