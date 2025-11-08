using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Scene Name")]
    public string gameSceneName = "Level 1"; // ganti sesuai nama scene gameplay

    // Tombol Start ditekan
    public void StartGame()
    {
        // SceneManager.LoadScene(gameSceneName);
        SceneManager.LoadScene(1);
    }

    // Tombol Exit ditekan
    public void ExitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // untuk Editor
        #else
        Application.Quit(); // untuk build
        #endif
    }
}
