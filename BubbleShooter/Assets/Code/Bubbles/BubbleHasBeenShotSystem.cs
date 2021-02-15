using Assets.Code.Bubbles.Solving;
using Assets.Code.DOTS;
using Assets.Code.Visuals;

namespace Assets.Code.Bubbles
{
    internal class BubbleHasBeenShotSystem : SystemBaseWithBarriers
    {
        protected override void OnCreate()
        {
            base.OnCreate();
            RequireSingletonForUpdate<SolverFinishedTagCmp>();
            RequireSingletonForUpdate<AllVisualsFinishedTagCmp>();
        }
        protected override void OnUpdate()
        {
            var beginInitBuffer = beginInitializationBuffer.CreateCommandBuffer();
            beginInitBuffer.CreateEntity(EntityManager.CreateArchetype(Archetypes.SpawnNextRow));
            beginInitBuffer.DestroyEntity(GetSingletonEntity<SolverFinishedTagCmp>());
        }
    }
}
