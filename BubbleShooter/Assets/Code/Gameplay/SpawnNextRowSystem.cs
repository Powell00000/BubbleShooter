using Unity.Entities;

namespace Assets.Code.Gameplay
{
    //[DisableAutoCreation]
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    internal class SpawnNextRowSystem : SystemBaseWithBarriers
    {
        [Zenject.Inject]
        private GameManager gameManager;

        protected override void OnCreate()
        {
            base.OnCreate();
            //RequireSingletonForUpdate<AllVisualsFinishedTagCmp>();
            //RequireSingletonForUpdate<SolverFinishedTagCmp>();
            RequireSingletonForUpdate<SpawnNextRowTagCmp>();
        }
        protected override void OnUpdate()
        {
            gameManager.SpawnRow();
            var endSimBuffer = endSimulationBuffer.CreateCommandBuffer();
            endSimBuffer.DestroyEntity(GetSingletonEntity<SpawnNextRowTagCmp>());
            endSimulationBuffer.AddJobHandleForProducer(Dependency);
        }
    }
}
