using UnityEngine;

[CreateAssetMenu(menuName = "Game settings")]
public class GameSettings : ScriptableObject
{
    [SerializeField]
    private float cellRadius;

    [SerializeField]
    private int numberOfCellsInEvenRow = 8;

    public float CellRadius => cellRadius;
    public int NumberOfCellsInEvenRow => numberOfCellsInEvenRow;
}
