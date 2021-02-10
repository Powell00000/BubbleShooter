using Assets.Code.Grid.Spawn;
using Assets.Code.Grid.Spawn.Hybrid;
using Assets.Code.Mono;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Transform spawnPosition;

    [Zenject.Inject]
    private GameSettings gameSettings;

    [Zenject.Inject]
    private CameraBounds cameraBounds;

    public bool IsEvenRow { get; set; }

    [ContextMenu("Spawn 1 row")]
    private void SpawnRow()
    {
        var entity = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntity();
        float cellDiameter = (cameraBounds.Right.x - cameraBounds.Left.x) / gameSettings.NumberOfCellsInEvenRow;

        World.DefaultGameObjectInjectionWorld.EntityManager.AddComponentData(entity, new SpawnRowCmp
        {
            CellCount = IsEvenRow == true ? gameSettings.NumberOfCellsInEvenRow : gameSettings.NumberOfCellsInEvenRow - 1,
            CellDiameter = cellDiameter,
            Position = spawnPosition.position
        });
        IsEvenRow = !IsEvenRow;
    }

    [ContextMenu("Spawn init rows")]
    private void SpawnRows()
    {
        float cellDiameter = (cameraBounds.Right.x - cameraBounds.Left.x) / gameSettings.NumberOfCellsInEvenRow;
        World.DefaultGameObjectInjectionWorld.GetExistingSystem<InitialSpawnSystem>()
            .SpawnInitialBoard(3, spawnPosition.position, gameSettings.NumberOfCellsInEvenRow, cellDiameter, this);
    }
}
