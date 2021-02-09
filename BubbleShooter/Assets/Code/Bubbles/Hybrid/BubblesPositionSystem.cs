using Unity.Entities;
using Unity.Transforms;

namespace Assets.Code.Bubbles.Hybrid
{
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    internal class BubblesPositionSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities
                .WithoutBurst()
                .ForEach((Entity e, Bubble hybridMono, in Translation translation) =>
                {
                    hybridMono.ForceUpdatePosition(translation.Value);
                })
                .Run();
        }
    }
}
