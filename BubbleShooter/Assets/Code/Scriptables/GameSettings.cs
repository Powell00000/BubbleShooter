using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(menuName = "Game settings")]
public class GameSettings : ScriptableObject
{
    [SerializeField]
    private int numberOfCellsInEvenRow = 8;

    public int NumberOfCellsInEvenRow => numberOfCellsInEvenRow;

    [SerializeField]
    private Color[] colors;

    public Color GetColorForNumber(int number)
    {
        int index = (int)math.log2(number) - 1;
        index = Mathf.Clamp(index, 0, colors.Length - 1);
        return colors[index];
    }
}
