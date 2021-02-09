using Assets.Code.Spawn;
using Unity.Entities;
using UnityEngine;

public class DebugMono : MonoBehaviour
{
    [SerializeField]
    private Transform spawnPosition;

    [SerializeField]
    private float radius;

    [SerializeField]
    private int numberOfCellsInEvenRow = 8;

    private bool isEvenRow = true;

    [ContextMenu("Spawn row")]
    private void SpawnRow()
    {
        var entity = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntity();
        World.DefaultGameObjectInjectionWorld.EntityManager.AddComponentData(entity, new SpawnRowCmp
        {
            CellCount = isEvenRow == true ? numberOfCellsInEvenRow : numberOfCellsInEvenRow - 1,
            CellRadius = radius,
            Position = spawnPosition.position
        });
        isEvenRow = !isEvenRow;
    }
}
