using Assets.Code.Grid.Hybrid;
using Unity.Entities;
using Unity.Transforms;

namespace Assets.Code.Bubbles.Hybrid
{
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    internal class BubblePositionSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities
                .WithoutBurst()
                .ForEach((Entity e, Bubble hybridMono, in Translation translation) =>
                {
                    hybridMono.SetPosition(translation.Value);
                })
                .Run();

            Entities
                .WithoutBurst()
                .ForEach((Entity e, Cell hybridMono, in Translation translation) =>
                {
                    hybridMono.SetPosition(translation.Value);
                })
                .Run();
        }
    }
}
