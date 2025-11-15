using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    [Header("Panels")]
    public CanvasGroup mainPanel;
    public CanvasGroup levelSelectPanel;
    public CanvasGroup aboutPanel;

    [Header("Backgrounds")]
    public GameObject mainBackground;
    public GameObject levelBackground;
    public GameObject aboutBackground;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioSource musicSource;
    public AudioClip clickSound;
    public AudioClip bgMusic;

    [Header("Fade Settings")]
    public float fadeDuration = 0.15f;

    CanvasGroup currentPanel;

    void Start()
    {
        if (musicSource != null && bgMusic != null)
        {
            musicSource.clip = bgMusic;
            musicSource.loop = true;
            musicSource.Play();
        }

        ShowMainMenu();
    }

    public void PlayClickSound()
    {
        if (audioSource != null && clickSound != null)
            audioSource.PlayOneShot(clickSound);
    }

    IEnumerator FadePanels(CanvasGroup newPanel)
    {
        CanvasGroup oldPanel = currentPanel;
        currentPanel = newPanel;

        if (oldPanel != null)
        {
            oldPanel.interactable = false;
            oldPanel.blocksRaycasts = false;

            // Fade Out
            for (float t = 0; t < fadeDuration; t += Time.deltaTime)
            {
                oldPanel.alpha = Mathf.Lerp(1, 0, t / fadeDuration);
                yield return null;
            }
            oldPanel.alpha = 0;
        }

        // Fade In
        newPanel.gameObject.SetActive(true);
        newPanel.interactable = true;
        newPanel.blocksRaycasts = true;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            newPanel.alpha = Mathf.Lerp(0, 1, t / fadeDuration);
            yield return null;
        }
        newPanel.alpha = 1;
    }

    public void ShowMainMenu()
    {
        PlayClickSound();

        SetBackground(mainBackground, levelBackground, aboutBackground);

        StartCoroutine(FadePanels(mainPanel));
    }

    public void ShowLevelSelect()
    {
        PlayClickSound();

        SetBackground(levelBackground, mainBackground, aboutBackground);

        StartCoroutine(FadePanels(levelSelectPanel));
    }

    public void ShowAboutMenu()
    {
        PlayClickSound();

        SetBackground(aboutBackground, mainBackground, levelBackground);

        StartCoroutine(FadePanels(aboutPanel));
    }

    void SetBackground(GameObject active, GameObject off1, GameObject off2)
    {
        active?.SetActive(true);
        off1?.SetActive(false);
        off2?.SetActive(false);
    }

    public void LoadLevel(string sceneName)
    {
        PlayClickSound();
        SceneManager.LoadScene(sceneName);
    }

    public void ExitGame()
    {
        PlayClickSound();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
