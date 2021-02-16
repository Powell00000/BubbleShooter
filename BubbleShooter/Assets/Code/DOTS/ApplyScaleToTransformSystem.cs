
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Assets.Code.DOTS
{
    internal class ApplyScaleToTransformSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities
                .WithoutBurst()
                .ForEach((Entity e, Transform transform, in Scale scaleCmp) =>
                {
                    transform.localScale = Vector3.one * scaleCmp.Value;
                })
                .Run();
        }
    }
}
