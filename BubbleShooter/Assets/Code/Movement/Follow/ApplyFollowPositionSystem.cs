using Unity.Entities;
using Unity.Transforms;

namespace Assets.Code.Movement.Follow
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    internal class ApplyFollowPositionSystem : SystemBaseWithBarriers
    {
        protected override void OnUpdate()
        {
            Entities
                .ForEach((Entity e, ref Translation translation, in FollowEntityCmp followCmp) =>
                {
                    translation.Value = followCmp.TargetPosition;
                })
                .Schedule();
        }
    }
}
