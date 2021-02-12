using Assets.Code.DOTS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Entities;

namespace Assets.Code.Bubbles.Hybrid
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    internal class BubbleDestroySystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities
                .WithoutBurst()
                .WithAll<DestroyTagCmp>()
                .ForEach((Entity e, Bubble bubbleMono) =>
                {
                    bubbleMono.Destroy();
                })
                .Run();
        }
    }
}
