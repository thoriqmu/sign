using UnityEngine;
using UnityEngine.SceneManagement;

public class PreGameManager : MonoBehaviour
{
    [Header("Scene Settings")]
    public string gameplaySceneName = "Level1";   // nama scene gameplay kamu

    // Dipanggil oleh tombol Start Game
    public void StartGame()
    {
        SceneManager.LoadScene(gameplaySceneName);
    }

    // Dipanggil oleh tombol Quit jika ingin
    public void QuitGame()
    {
        Application.Quit();
    }
}
