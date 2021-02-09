using Assets.Code.Grid.Spawn;
using Unity.Entities;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Transform spawnPosition;

    [Zenject.Inject]
    private GameSettings gameSettings;

    private bool isEvenRow = true;

    [ContextMenu("Spawn row")]
    private void SpawnRow()
    {
        var entity = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntity();
        World.DefaultGameObjectInjectionWorld.EntityManager.AddComponentData(entity, new SpawnRowCmp
        {
            CellCount = isEvenRow == true ? gameSettings.NumberOfCellsInEvenRow : gameSettings.NumberOfCellsInEvenRow - 1,
            CellRadius = gameSettings.CellRadius,
            Position = spawnPosition.position
        });
        isEvenRow = !isEvenRow;
    }
}
