using Unity.Entities;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGameplayScene : MonoBehaviour
{
    private void Start()
    {
        DefaultWorldInitialization.Initialize("Default World");
        SceneManager.LoadScene(1);
    }
}
