using Assets.Code.Grid.Cells;
using Unity.Entities;
using Unity.Transforms;

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
                .WithAll<CellCmp, SpawnBubbleTagCmp>()
                .ForEach((Entity e, in Scale scaleCmp) =>
                {
                    var bubble = UnityEngine.Object.Instantiate(bubblePrefab).GetComponent<Bubble>();
                    bubble.CreateAndSetupBubbleEntity(e, scaleCmp);
                    beginSimBuffer.RemoveComponent<SpawnBubbleTagCmp>(e);
                })
                .Run();

            beginSimulationBuffer.AddJobHandleForProducer(Dependency);
        }
    }
}
