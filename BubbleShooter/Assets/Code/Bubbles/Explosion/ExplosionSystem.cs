
using Assets.Code.Bubbles.Hybrid;
using Assets.Code.DOTS;
using Unity.Entities;

namespace Assets.Code.Bubbles.Explosion
{
    internal class ExplosionSystem : SystemBaseWithBarriers
    {
        protected override void OnUpdate()
        {
            Entities
                .WithStructuralChanges()
                .WithoutBurst()
                .WithAll<ExplodeTagCmp>()
                .WithNone<DestroyTagCmp>()
                .ForEach((Entity e, Bubble bubbleMono) =>
                {
                    bubbleMono.Explode();
                })
                .Run();
        }
    }
}
