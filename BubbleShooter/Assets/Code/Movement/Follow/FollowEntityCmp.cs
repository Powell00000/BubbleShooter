using Unity.Entities;
using Unity.Mathematics;

namespace Assets.Code.Movement.Follow
{
    [GenerateAuthoringComponent]
    internal struct FollowEntityCmp : IComponentData
    {
        public Entity EntityToFollow;
        public float3 UpdatedPosition;
    }
}
