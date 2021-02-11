using Assets.Code.Grid.Spawn;
using Assets.Code.Grid.Spawn.Hybrid;
using Assets.Code.Mono;
using Unity.Entities;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Transform spawnPosition;

    [SerializeField]
    private Transform leftWall;

    [SerializeField]
    private Transform rightWall;

    [SerializeField]
    private Transform topWall;

    [Zenject.Inject]
    private GameSettings gameSettings;

    [Zenject.Inject]
    private CameraBounds cameraBounds;

    public bool IsEvenRow { get; set; }

    private static float calculatedCellDiameter;
    private static bool initialized = false;
    private static int maxRowsCount;

    public static float CellDiameter => calculatedCellDiameter;
    public static bool Initialized => initialized;
    public static int MaxRowsCount => maxRowsCount;

    private void Start()
    {
        InitializeCameraBounds();
        SetupWalls();
        CalculateCellDiameter();
        CalculateMaxRowsCount();
        initialized = true;
    }

    private void CalculateCellDiameter()
    {
        calculatedCellDiameter = (cameraBounds.Right.x - cameraBounds.Left.x) / gameSettings.NumberOfCellsInEvenRow;
    }

    private void CalculateMaxRowsCount()
    {
        float height = Vector3.Distance(cameraBounds.Top, cameraBounds.Bottom);
        maxRowsCount = Mathf.FloorToInt(height / calculatedCellDiameter);

        Debug.Log($"{nameof(maxRowsCount)} = {maxRowsCount}");
    }

    private void InitializeCameraBounds()
    {
        cameraBounds.Initialize();
    }

    private void SetupWalls()
    {
        leftWall.position = cameraBounds.Left;
        rightWall.position = cameraBounds.Right;
        topWall.position = cameraBounds.Top;
    }

    [ContextMenu("Spawn 1 row")]
    private void SpawnRow()
    {
        var entity = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntity();
        World.DefaultGameObjectInjectionWorld.EntityManager.AddComponentData(entity, new SpawnRowCmp
        {
            CellCount = IsEvenRow == true ? gameSettings.NumberOfCellsInEvenRow : gameSettings.NumberOfCellsInEvenRow - 1,
            CellDiameter = calculatedCellDiameter,
            Position = spawnPosition.position
        });
        IsEvenRow = !IsEvenRow;
    }

    [ContextMenu("Spawn init rows")]
    private void SpawnRows()
    {
        World.DefaultGameObjectInjectionWorld.GetExistingSystem<InitialSpawnSystem>()
            .SpawnInitialBoard(3, spawnPosition.position, gameSettings.NumberOfCellsInEvenRow, calculatedCellDiameter, this);
    }
}
