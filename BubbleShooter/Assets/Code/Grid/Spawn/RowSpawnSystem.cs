using Assets.Code.DOTS;
using Assets.Code.Grid;
using Assets.Code.Mono;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Assets.Code.Grid.Spawn
{
    internal class RowSpawnSystem : SystemBaseWithBarriers
    {
        private struct CellPosition : IGridPosition
        {
            public float3 Position { get; set; }
        }

        [Zenject.Inject]
        private GridCellBehaviour gridCellPrefab = null;

        protected override void OnCreate()
        {
            base.OnCreate();
            RequireSingletonForUpdate<SpawnRowCmp>();
        }

        protected override void OnUpdate()
        {
            SpawnRowCmp spawnRowCmp = GetSingleton<SpawnRowCmp>();

            CellPosition[] row = GridEx.GetCellsPositionsInARow<CellPosition>(spawnRowCmp.CellRadius, spawnRowCmp.Position, spawnRowCmp.CellCount);

            var beginSimBuffer = beginSimulationBuffer.CreateCommandBuffer();

            for (int cellNumber = 0; cellNumber < row.Length; cellNumber++)
            {
                var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null);
                var prefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(gridCellPrefab.gameObject, settings);

                var entity = beginSimBuffer.Instantiate(prefab);
                beginSimBuffer.SetComponent(entity, new Translation { Value = row[cellNumber].Position });
            }

            beginSimBuffer.DestroyEntity(GetSingletonEntity<SpawnRowCmp>());
            beginSimBuffer.CreateEntity(EntityManager.CreateArchetype(Archetypes.RowSpawned));
        }
    }
}
