using Assets.Code.Visuals;
using Unity.Entities;

namespace Assets.Code.Bubbles.Connections
{
    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
    internal class RemoveTopRowConnectionTagsSystem : SystemBaseWithBarriers
    {
        protected override void OnCreate()
        {
            base.OnCreate();
        }
        protected override void OnUpdate()
        {
            var beginInitBuffer = beginInitializationBuffer.CreateCommandBuffer();
            Entities
                .WithAll<HasConnectionWithTopRowTagCmp>()
                .ForEach((Entity e) =>
                {
                    beginInitBuffer.RemoveComponent<HasConnectionWithTopRowTagCmp>(e);
                })
                .Schedule();

            //endSimBuffer.DestroyEntity(GetSingletonEntity<RefreshConnectionsTagCmp>());

            beginInitializationBuffer.AddJobHandleForProducer(Dependency);
        }
    }
}
