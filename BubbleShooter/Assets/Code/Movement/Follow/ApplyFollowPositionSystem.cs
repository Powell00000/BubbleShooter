using Assets.Code.DOTS;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Assets.Code.Movement.Follow
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    internal class ApplyFollowPositionSystem : SystemBaseWithBarriers
    {
        protected override void OnUpdate()
        {
            float dt = Time.DeltaTime;
            var beginInitBuffer = beginInitializationBuffer.CreateCommandBuffer();
            Entities
                .WithNone<DestroyTagCmp>()
                .ForEach((Entity e, ref Translation translation, in FollowEntityCmp followCmp) =>
                {
                    float distance = math.distance(translation.Value, followCmp.TargetPosition);
                    float3 moveVector = math.normalizesafe(followCmp.TargetPosition - translation.Value) * dt;
                    float moveDistance = math.length(moveVector);

                    if (moveDistance >= distance)
                    {
                        translation.Value = followCmp.TargetPosition;
                        beginInitBuffer.RemoveComponent<IsMovingTagCmp>(e);
                    }
                    else
                    {
                        translation.Value += moveVector;
                        beginInitBuffer.AddComponent<IsMovingTagCmp>(e);
                    }
                })
                .Schedule();

            beginInitializationBuffer.AddJobHandleForProducer(Dependency);
        }
    }
}
