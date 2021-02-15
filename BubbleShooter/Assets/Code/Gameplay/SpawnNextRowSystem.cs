
using Assets.Code.Visuals;
using Unity.Entities;

namespace Assets.Code.Gameplay
{
    [DisableAutoCreation]
    internal class SpawnNextRowSystem : SystemBaseWithBarriers
    {
        [Zenject.Inject]
        private GameManager gameManager;

        protected override void OnCreate()
        {
            base.OnCreate();
            RequireSingletonForUpdate<AllVisualsFinishedTagCmp>();
        }
        protected override void OnUpdate()
        {
            gameManager.SpawnRow();
            var endSimBuffer = endSimulationBuffer.CreateCommandBuffer();
            endSimBuffer.DestroyEntity(GetSingletonEntity<AllVisualsFinishedTagCmp>());
            endSimulationBuffer.AddJobHandleForProducer(Dependency);
        }
    }
}
