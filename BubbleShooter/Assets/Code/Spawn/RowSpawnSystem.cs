using Assets.Code.Grid;
using Assets.Code.Mono;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Assets.Code.Spawn
{
    class RowSpawnSystem : SystemBase
    {
        struct CellPosition : IGridPosition
        {
            public float3 Position { get; set; }
        }

        [Zenject.Inject]
        GridCellBehaviour gridCellPrefab;

        protected override void OnCreate()
        {
            RequireSingletonForUpdate<SpawnRowTagCmp>();
        }

        protected override void OnUpdate()
        {
            float radius = 1.5f;
            float3 startingCenteredPosition = float3.zero;
            int numberOfCells = 5;
            CellPosition[] row = GridManager.GetRowData<CellPosition>(radius, startingCenteredPosition, numberOfCells);

            for (int cellNumber = 0; cellNumber < row.Length; cellNumber++)
            {
                var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null);
                var prefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(gridCellPrefab.gameObject, settings);

                var entity = EntityManager.Instantiate(prefab);
                EntityManager.SetName(entity, "GridCell");
                EntityManager.SetComponentData(entity, new Translation { Value = row[cellNumber].Position });
            }

            EntityManager.DestroyEntity(GetSingletonEntity<SpawnRowTagCmp>());
        }
    }
}
