using Assets.Code.Spawn;
using Unity.Entities;
using UnityEngine;

public class DebugMono : MonoBehaviour
{


    [ContextMenu("Spawn row")]
    void SpawnRow()
    {
        var entity = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntity();
        World.DefaultGameObjectInjectionWorld.EntityManager.AddComponentData(entity, new SpawnRowTagCmp());
    }
}
