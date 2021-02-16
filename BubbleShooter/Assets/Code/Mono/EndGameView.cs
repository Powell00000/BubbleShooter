using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class EndGameView : MonoBehaviour
{
    [SerializeField]
    private Canvas canvas;

    [SerializeField]
    private Button restartButton;

    public void Initialize(GameManager gameManager)
    {
        transform.localScale = Vector3.zero;
        canvas.enabled = false;
        gameManager.OnGameEnded += OnGameEnded;

        restartButton.onClick.AddListener(gameManager.RestartGame);
    }

    private void OnGameEnded()
    {
        canvas.enabled = true;
        transform.DOScale(Vector3.one, 0.2f);
    }
}
