
using Assets.Code.Bubbles.Hybrid;
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
                .ForEach((Entity e, Bubble bubbleMono) =>
                {
                    bubbleMono.Explode();
                })
                .Run();
        }
    }
}
