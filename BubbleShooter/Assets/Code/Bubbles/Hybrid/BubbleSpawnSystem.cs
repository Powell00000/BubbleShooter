using Assets.Code.Bubbles.Solving;
using Assets.Code.Grid.Cells;
using Assets.Code.Grid.Row;
using Assets.Code.Grid.Spawn.Hybrid;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Assets.Code.Bubbles.Hybrid
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    [UpdateAfter(typeof(RowSpawnSystem))]
    internal class BubbleSpawnSystem : SystemBaseWithBarriers
    {
        [Zenject.Inject]
        private Bubble bubblePrefab;

        [Zenject.Inject]
        private GameManager gameManager;

        protected override void OnUpdate()
        {
            var beginInitBuffer = beginInitializationBuffer.CreateCommandBuffer();

            Entities
                .WithoutBurst()
                .WithStructuralChanges()
                .ForEach((Entity e, ref CellCmp cellCmp, in Scale scaleCmp, in SpawnBubbleCmp spawnBubbleCmp, in Translation translation) =>
                {
                    var bubble = UnityEngine.Object.Instantiate(bubblePrefab, translation.Value, quaternion.identity).GetComponent<Bubble>();
                    bubble.CreateAndSetupBubbleEntity(e, scaleCmp);

                    if (spawnBubbleCmp.SolveHere)
                    {
                        beginInitBuffer.AddComponent(bubble.Entity, new SolveHereTagCmp());
                    }

                    if (spawnBubbleCmp.RandomizeNumber)
                    {
                        beginInitBuffer.SetComponent(bubble.Entity, new NumberCmp { Value = gameManager.GetRandomBubbleNumber() });
                    }
                    else
                    {
                        beginInitBuffer.SetComponent(bubble.Entity, new NumberCmp { Value = spawnBubbleCmp.Number });
                    }

                    beginInitBuffer.SetComponent(bubble.Entity, EntityManager.GetComponentData<RowSharedCmp>(e));

                    cellCmp.OccupyingEntity = bubble.Entity;

                    beginInitBuffer.RemoveComponent<SpawnBubbleCmp>(e);
                })
                .Run();

            //endSimBuffer.CreateEntity(EntityManager.CreateArchetype(Archetypes.RefreshConnections));

            beginInitializationBuffer.AddJobHandleForProducer(Dependency);
        }
    }
}
