using Unity.Entities;

namespace Assets.Code.Bubbles.Hybrid
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    internal class RefreshNumberSystem : SystemBaseWithBarriers
    {
        protected override void OnUpdate()
        {
            Entities
                .WithStructuralChanges()
                .WithoutBurst()
                .ForEach((Entity e, Bubble bubbleMono, in NumberCmp numberCmp) =>
                {
                    bubbleMono.RefreshNumber(numberCmp.Value);
                })
                .Run();
        }
    }
}
