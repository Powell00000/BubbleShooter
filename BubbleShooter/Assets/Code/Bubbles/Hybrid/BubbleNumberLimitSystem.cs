
using Assets.Code.Bubbles.Explosion;
using Assets.Code.Bubbles.Nodes;
using Assets.Code.DOTS;
using Unity.Entities;

namespace Assets.Code.Bubbles.Hybrid
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    internal class BubbleNumberLimitSystem : SystemBaseWithBarriers
    {
        protected override void OnUpdate()
        {
            var destroy = GetComponentDataFromEntity<DestroyTagCmp>();
            var beginInitBuffer = beginInitializationBuffer.CreateCommandBuffer();
            Entities
                .WithDisposeOnCompletion(destroy)
                .WithNone<ExplodeTagCmp, DestroyTagCmp>()
                .ForEach((Entity e, in NumberCmp numberCmp, in DynamicBuffer<NodeNeighboursCmp> neighbours) =>
                {
                    if (numberCmp.Value >= 2048)
                    {
                        beginInitBuffer.AddComponent<ExplodeTagCmp>(e);

                        for (int i = 0; i < neighbours.Length; i++)
                        {
                            if (!destroy.HasComponent(neighbours[i].Neighbour))
                            {
                                beginInitBuffer.AddComponent(neighbours[i].Neighbour, new DestroyTagCmp());
                            }
                        }
                    }
                })
                .Schedule();
            beginInitializationBuffer.AddJobHandleForProducer(Dependency);
        }
    }
}
