using Assets.Code.Bubbles;
using Assets.Code.DOTS;
using Assets.Code.Gameplay;
using Assets.Code.Grid.Spawn;
using Assets.Code.Movement;
using Assets.Code.Movement.Follow;
using Unity.Entities;

namespace Assets.Code.Visuals
{
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    internal class VisualsFinishedSystem : SystemBaseWithBarriers
    {
        private EntityQueryDesc visualsQueryDesc = new EntityQueryDesc
        {
            Any = new ComponentType[]
            {
                typeof(MoveDownTagCmp),
                typeof(BubbleIsShootingTagCmp),
                typeof(IsMovingTagCmp),
                typeof(IsAnimatingTagCmp),
                typeof(SpawnBubbleCmp),
                typeof(JustSpawnedTagCmp),
                typeof(SpawnRowCmp),
                typeof(SpawnNextRowTagCmp),
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
            var beginInitBuffer = beginInitializationBuffer.CreateCommandBuffer();

            bool visualsInProgress = HasSingleton<VisualsInProgressTagCmp>();
            bool anyVisualsPlaying = !visualsQuery.IsEmpty;

            if (!visualsInProgress && anyVisualsPlaying)
            {
                beginInitBuffer.CreateEntity(EntityManager.CreateArchetype(Archetypes.VisualsInProgress));
                if (HasSingleton<AllVisualsFinishedTagCmp>())
                {
                    beginInitBuffer.DestroyEntity(GetSingletonEntity<AllVisualsFinishedTagCmp>());
                }
            }

            if (visualsInProgress && !anyVisualsPlaying)
            {
                beginInitBuffer.DestroyEntity(GetSingletonEntity<VisualsInProgressTagCmp>());
                beginInitBuffer.CreateEntity(EntityManager.CreateArchetype(Archetypes.AllVisualsFinished));
            }

            beginInitializationBuffer.AddJobHandleForProducer(Dependency);
        }
    }
}
