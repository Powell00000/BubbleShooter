using Unity.Entities;

namespace Assets.Code.Bubbles.Connections
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    internal class RemoveTopRowConnectionTagsSystem : SystemBaseWithBarriers
    {
        protected override void OnCreate()
        {
            base.OnCreate();
            //RequireSingletonForUpdate<RefreshConnectionsTagCmp>();
        }
        protected override void OnUpdate()
        {
            var beginInitBuffer = beginInitializationBuffer.CreateCommandBuffer();
            var endSimBuffer = endSimulationBuffer.CreateCommandBuffer();
            Entities
                .WithAll<HasConnectionWithTopRowTagCmp>()
                .ForEach((Entity e) =>
                {
                    beginInitBuffer.RemoveComponent<HasConnectionWithTopRowTagCmp>(e);
                })
                .Schedule();

            //endSimBuffer.DestroyEntity(GetSingletonEntity<RefreshConnectionsTagCmp>());

            beginInitializationBuffer.AddJobHandleForProducer(Dependency);
            endSimulationBuffer.AddJobHandleForProducer(Dependency);
        }
    }
}
