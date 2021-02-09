using Assets.Code.Grid;
using Unity.Entities;

namespace Assets.Code.Bubbles.Hybrid
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    internal class BubbleSpawnSystem : SystemBaseWithBarriers
    {
        [Zenject.Inject]
        private Bubble bubblePrefab;

        protected override void OnUpdate()
        {
            var beginSimBuffer = beginSimulationBuffer.CreateCommandBuffer();
            Entities
                .WithoutBurst()
                .WithStructuralChanges()
                .WithAll<CellCmp, SpawnBubbleCmp>()
                .ForEach((Entity e) =>
                {
                    var bubble = UnityEngine.Object.Instantiate(bubblePrefab).GetComponent<Bubble>();
                    bubble.CreateAndSetupBubbleEntity(e);
                    beginSimBuffer.RemoveComponent<SpawnBubbleCmp>(e);
                })
                .Run();

            beginSimulationBuffer.AddJobHandleForProducer(Dependency);
        }
    }
}
