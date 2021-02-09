using UnityEngine;

[CreateAssetMenu(menuName = "Game settings")]
public class GameSettings : ScriptableObject
{
    [SerializeField]
    private int numberOfCellsInEvenRow = 8;

    public int NumberOfCellsInEvenRow => numberOfCellsInEvenRow;
}
