using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Assets.Code.Bubbles.Nodes
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    internal class BubbleNodeUpdateSystem : SystemBaseWithBarriers
    {
        private EntityQuery allBubbles;
        private EntityQueryDesc bubblesQueryDesc = new EntityQueryDesc()
        {
            All = new ComponentType[] { ComponentType.ReadOnly<BubbleCmp>(), ComponentType.ReadOnly<Translation>() }
        };

        protected override void OnCreate()
        {
            base.OnCreate();
            allBubbles = GetEntityQuery(bubblesQueryDesc);
        }
        protected override void OnUpdate()
        {
            Entities
                .WithAll<BubbleCmp>()
                .WithoutBurst()
                .ForEach((Entity e, ref DynamicBuffer<NodeNeighboursCmp> neighboursCmp, in Translation translation) =>
                {
                    NativeArray<NodeNeighboursCmp> nodeNeighbours = GetNeighbours(translation.Value, e);
                    neighboursCmp.Clear();
                    neighboursCmp.AddRange(nodeNeighbours);
                    nodeNeighbours.Dispose();
                })
                .Run();
        }

        private NativeArray<NodeNeighboursCmp> GetNeighbours(float3 position, Entity entity)
        {
            NativeList<NodeNeighboursCmp> neighbours = new NativeList<NodeNeighboursCmp>(6, Allocator.TempJob);
            float cellDiameter = GameManager.CellDiameter;

            var entities = allBubbles.ToEntityArray(Allocator.TempJob);
            var translations = allBubbles.ToComponentDataArray<Translation>(Allocator.TempJob);

            for (int i = 0; i < entities.Length; i++)
            {
                if (entities[i] != entity)
                {
                    if (math.distance(position, translations[i].Value) <= cellDiameter * 1.5f)
                    {
                        neighbours.Add(new NodeNeighboursCmp { Neighbour = entities[i] });
                    }
                }
            }
            return neighbours;
        }
    }
}
