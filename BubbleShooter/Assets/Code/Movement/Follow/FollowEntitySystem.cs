
using Assets.Code.DOTS;
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
                .WithNone<DestroyTagCmp>()
                .ForEach((Entity e, ref FollowEntityCmp followCmp) =>
                {
                    if (followCmp.EntityToFollow == Entity.Null)
                    {
                        return;
                    }
                    if (!translations.HasComponent(followCmp.EntityToFollow))
                    {
                        followCmp.EntityToFollow = Entity.Null;
                        return;
                    }
                    followCmp.TargetPosition = translations[followCmp.EntityToFollow].Value;
                })
                .WithDisposeOnCompletion(translations)
                .Schedule();
        }
    }
}
