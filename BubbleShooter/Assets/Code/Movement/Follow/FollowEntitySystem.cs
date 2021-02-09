
using Unity.Entities;
using Unity.Transforms;

namespace Assets.Code.Movement.Follow
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    internal class FollowEntitySystem : SystemBaseWithBarriers
    {
        protected override void OnUpdate()
        {
            var translations = GetComponentDataFromEntity<Translation>(true);
            Entities
                .WithReadOnly(translations)
                .ForEach((Entity e, ref FollowEntityCmp followCmp) =>
                {
                    followCmp.UpdatedPosition = translations[followCmp.EntityToFollow].Value;
                })
                .WithDisposeOnCompletion(translations)
                .Schedule();
        }
    }
}
