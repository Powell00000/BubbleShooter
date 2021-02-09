using Assets.Code.DOTS;
using Assets.Code.Movement;
using Unity.Entities;

namespace Assets.Code.Visuals
{
    internal class VisualFinishedSystem : SystemBaseWithBarriers
    {
        private EntityQueryDesc visualsQueryDesc = new EntityQueryDesc
        {
            Any = new ComponentType[]
            {
                typeof(MoveDownCmp),
            }
        };
        private EntityQuery visualsQuery;

        protected override void OnCreate()
        {
            base.OnCreate();
            visualsQuery = GetEntityQuery(visualsQueryDesc);
        }

        protected override void OnUpdate()
        {
            var beginSimBuffer = beginSimulationBuffer.CreateCommandBuffer();
            var endSimBuffer = endSimulationBuffer.CreateCommandBuffer();

            bool visualsInProgress = HasSingleton<VisualsInProgressTagCmp>();

            if (!visualsInProgress && !visualsQuery.IsEmpty)
            {
                beginSimBuffer.CreateEntity(EntityManager.CreateArchetype(Archetypes.VisualsInProgress));
            }

            if (visualsInProgress && visualsQuery.IsEmpty)
            {
                endSimBuffer.DestroyEntity(GetSingletonEntity<VisualsInProgressTagCmp>());
            }
        }
    }
}
