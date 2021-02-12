using Unity.Entities;

namespace Assets.Code.Bubbles.Connections
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    internal class RemoveTopRowConnectionTagsSystem : SystemBaseWithBarriers
    {
        protected override void OnCreate()
        {
            base.OnCreate();
            RequireSingletonForUpdate<RefreshConnectionsTagCmp>();
        }
        protected override void OnUpdate()
        {
            var endSimBuffer = endSimulationBuffer.CreateCommandBuffer();
            Entities
                .WithAll<HasConnectionWithTopRowTagCmp>()
                .ForEach((Entity e) =>
                {
                    endSimBuffer.RemoveComponent<HasConnectionWithTopRowTagCmp>(e);
                })
                .Schedule();

            EntityManager.DestroyEntity(GetSingletonEntity<RefreshConnectionsTagCmp>());
            endSimulationBuffer.AddJobHandleForProducer(Dependency);
        }
    }
}
