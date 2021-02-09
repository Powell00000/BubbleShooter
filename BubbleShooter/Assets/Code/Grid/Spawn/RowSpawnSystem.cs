using Assets.Code.DOTS;
using Assets.Code.Mono;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Assets.Code.Grid.Spawn
{
    internal class RowSpawnSystem : SystemBaseWithBarriers
    {
        private struct CellPosition : ICellData
        {
            public float3 Position { get; set; }
            public float Diameter { get; set; }
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

            CellPosition[] row = GridEx.GetCellsPositionsInARow<CellPosition>(spawnRowCmp.CellDiameter, spawnRowCmp.Position, spawnRowCmp.CellCount);

            var beginSimBuffer = beginSimulationBuffer.CreateCommandBuffer();

            for (int cellNumber = 0; cellNumber < row.Length; cellNumber++)
            {
                var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null);
                var prefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(gridCellPrefab.gameObject, settings);

                var entity = beginSimBuffer.Instantiate(prefab);
                beginSimBuffer.SetComponent(entity, new Translation { Value = row[cellNumber].Position });
                beginSimBuffer.SetComponent(entity, new CellCmp { Diameter = row[cellNumber].Diameter });
                beginSimBuffer.AddComponent(entity, new Scale { Value = row[cellNumber].Diameter });
            }

            beginSimBuffer.DestroyEntity(GetSingletonEntity<SpawnRowCmp>());
            beginSimBuffer.CreateEntity(EntityManager.CreateArchetype(Archetypes.RowSpawned));
        }
    }
}
