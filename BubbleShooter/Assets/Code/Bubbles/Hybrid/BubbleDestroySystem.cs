using Assets.Code.DOTS;
using Assets.Code.Visuals;
using Unity.Entities;

namespace Assets.Code.Bubbles.Hybrid
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    internal class BubbleDestroySystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities
                .WithoutBurst()
                .WithAll<DestroyTagCmp>()
                .WithNone<IsAnimatingTagCmp>()
                .WithStructuralChanges()
                .ForEach((Entity e, Bubble bubbleMono) =>
                {
                    bubbleMono.Destroy();
                })
                .Run();
        }
    }
}
