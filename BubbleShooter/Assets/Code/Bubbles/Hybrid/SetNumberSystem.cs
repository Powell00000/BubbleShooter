
using Assets.Code.DOTS;
using Unity.Entities;
using Unity.Mathematics;

namespace Assets.Code.Bubbles.Hybrid
{
    internal class SetNumberSystem : SystemBase
    {
        private Random random;
        protected override void OnCreate()
        {
            base.OnCreate();
            random = new Random(2);
        }
        protected override void OnUpdate()
        {
            Entities
                .WithoutBurst()
                .WithAll<JustSpawnedTagCmp>()
                .ForEach((Entity e, ref NumberCmp numberCmp) =>
                {
                    numberCmp.Value = (int)math.pow(2, random.NextInt(1, 4));
                })
                .Run();
        }
    }
}
