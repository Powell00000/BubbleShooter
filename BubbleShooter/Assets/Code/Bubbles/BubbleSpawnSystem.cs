
using Assets.Code.Grid;
using Assets.Code.Movement.Follow;
using Unity.Entities;

namespace Assets.Code.Bubbles
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    internal class BubbleSpawnSystem : SystemBaseWithBarriers
    {
        [Zenject.Inject]
        private Bubble bubblePrefab;

        protected override void OnUpdate()
        {
            var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null);
            var prefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(bubblePrefab.gameObject, settings);

            var beginPresBuffer = beginPresentationBuffer.CreateCommandBuffer();
            var beginSimBuffer = beginSimulationBuffer.CreateCommandBuffer();
            Entities
                .WithAll<CellCmp, SpawnBubbleCmp>()
                .ForEach((Entity e) =>
                {
                    var entity = beginPresBuffer.Instantiate(prefab);
                    beginPresBuffer.SetComponent(entity, new FollowEntityCmp { EntityToFollow = e });
                    beginSimBuffer.RemoveComponent<SpawnBubbleCmp>(e);
                })
                .Schedule();

            beginPresentationBuffer.AddJobHandleForProducer(Dependency);
            beginSimulationBuffer.AddJobHandleForProducer(Dependency);
        }
    }
}
