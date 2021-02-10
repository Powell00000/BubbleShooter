
using Unity.Entities;

namespace Assets.Code.DOTS
{
    internal class UnmarAsSpawnedSystem : SystemBaseWithBarriers
    {
        protected override void OnUpdate()
        {
            var beginInitBuffer = beginInitializationBuffer.CreateCommandBuffer();
            Entities
                .WithAll<JustSpawnedTagCmp>()
                .ForEach((Entity e) =>
                {
                    beginInitBuffer.RemoveComponent<JustSpawnedTagCmp>(e);
                })
                .Schedule();

            beginInitializationBuffer.AddJobHandleForProducer(Dependency);
        }
    }
}
