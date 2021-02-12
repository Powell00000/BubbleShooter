using Assets.Code.Bubbles;
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

    [SerializeField]
    private Transform leftWall;

    [SerializeField]
    private Transform rightWall;

    [SerializeField]
    private Transform topWall;

    [Zenject.Inject]
    private Cannon cannon;

    [Zenject.Inject]
    private GameSettings gameSettings;

    [Zenject.Inject]
    private CameraBounds cameraBounds;

    private Unity.Mathematics.Random random;

    public bool IsEvenRow { get; set; }

    private static float calculatedCellDiameter;
    private static bool initialized = false;
    private static int maxRowsCount;

    public static float CellDiameter => calculatedCellDiameter;
    public static bool Initialized => initialized;
    public static int MaxRowsCount => maxRowsCount;

    private void Start()
    {
        random = new Unity.Mathematics.Random(2);
        Application.targetFrameRate = 60;
        IsEvenRow = true;
        InitializeCameraBounds();
        SetupWalls();
        CalculateCellDiameter();
        CalculateMaxRowsCount();
        SpawnInitGrid();
        SpawnInitBubbles();
        InitializeCannon();
        initialized = true;
    }

    public int GetRandomBubbleNumber()
    {
        return (int)math.pow(2, random.NextInt(1, 4));
    }

    private void InitializeCannon()
    {
        cannon.Initialize(this);
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
        spawnPosition.position = cameraBounds.Top;
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
    private void SpawnInitGrid()
    {
        World.DefaultGameObjectInjectionWorld.GetExistingSystem<SpawnGridSystem>()
            .SpawnInitialBoard(spawnPosition.position, gameSettings.NumberOfCellsInEvenRow, calculatedCellDiameter, this);
    }

    [ContextMenu("Spawn init bubbles")]
    private void SpawnInitBubbles()
    {
        for (int i = 0; i < 3; i++)
        {
            var entity = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntity();
            World.DefaultGameObjectInjectionWorld.EntityManager.AddComponentData(entity, new PopulateRowWithBubbles { Row = i, RandomizeNumbers = true });
        }
    }
}
