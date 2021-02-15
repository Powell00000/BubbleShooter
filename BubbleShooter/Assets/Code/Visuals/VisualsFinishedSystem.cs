using Assets.Code.Bubbles;
using Assets.Code.DOTS;
using Assets.Code.Movement;
using Assets.Code.Movement.Follow;
using Unity.Entities;

namespace Assets.Code.Visuals
{
    internal class VisualsFinishedSystem : SystemBaseWithBarriers
    {
        private EntityQueryDesc visualsQueryDesc = new EntityQueryDesc
        {
            Any = new ComponentType[]
            {
                typeof(MoveDownTagCmp),
                typeof(BubbleIsShootingTagCmp),
                typeof(IsMovingTagCmp)
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

            bool visualsInProgress = HasSingleton<VisualsInProgressTagCmp>();
            bool anyVisualsPlaying = !visualsQuery.IsEmpty;

            if (!visualsInProgress && anyVisualsPlaying)
            {
                beginSimBuffer.CreateEntity(EntityManager.CreateArchetype(Archetypes.VisualsInProgress));
                if (HasSingleton<AllVisualsFinishedTagCmp>())
                {
                    beginSimBuffer.DestroyEntity(GetSingletonEntity<AllVisualsFinishedTagCmp>());
                }
            }

            if (visualsInProgress && !anyVisualsPlaying)
            {
                beginSimBuffer.DestroyEntity(GetSingletonEntity<VisualsInProgressTagCmp>());
                beginSimBuffer.CreateEntity(EntityManager.CreateArchetype(Archetypes.AllVisualsFinished));
            }

            beginSimulationBuffer.AddJobHandleForProducer(Dependency);
        }
    }
}
