using Unity.Entities;

namespace Assets.Code.Bubbles.Hybrid
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    internal class RefreshNumberSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities
                .WithoutBurst()
                .ForEach((Entity e, Bubble bubbleMono, in NumberCmp numberCmp) =>
                {
                    bubbleMono.RefreshNumber(numberCmp.Value);
                })
                .Run();
        }
    }
}
