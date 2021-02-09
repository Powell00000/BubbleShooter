using UnityEngine;

[CreateAssetMenu(menuName = "Game settings")]
public class GameSettings : ScriptableObject
{
    [SerializeField]
    private float cellRadius;

    public float CellRadius => cellRadius;
}
