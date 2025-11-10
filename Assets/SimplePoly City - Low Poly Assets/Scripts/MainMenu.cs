using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainPanel;           // Panel berisi tombol Start & Exit
    public GameObject levelSelectPanel;    // Panel berisi tombol Level 1–4

    [Header("Backgrounds")]
    public GameObject mainBackground;      // Gambar background menu utama
    public GameObject levelBackground;     // Gambar background level select

    void Start()
    {
        ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        mainPanel.SetActive(true);
        levelSelectPanel.SetActive(false);

        if (mainBackground != null) mainBackground.SetActive(true);
        if (levelBackground != null) levelBackground.SetActive(false);
    }

    public void ShowLevelSelect()
    {
        mainPanel.SetActive(false);
        levelSelectPanel.SetActive(true);

        if (mainBackground != null) mainBackground.SetActive(false);
        if (levelBackground != null) levelBackground.SetActive(true);
    }

    public void LoadLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
