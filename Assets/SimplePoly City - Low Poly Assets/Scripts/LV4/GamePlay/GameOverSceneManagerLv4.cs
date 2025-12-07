using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOverSceneManagerLv4 : MonoBehaviour
{
    [Header("Panels")]
    public CanvasGroup gameOverPanel;

    [Header("Audio")]
    public AudioSource audioSource;     // SFX (klik tombol)
    public AudioSource musicSource;     // Musik background
    public AudioClip clickSound;        // Suara klik
    public AudioClip bgMusic;           // Lagu background

    [Header("Fade Settings")]
    public float fadeDuration = 0.2f;

    void Start()
    {
        // Mainkan musik background
        if (musicSource != null && bgMusic != null)
        {
            musicSource.clip = bgMusic;
            musicSource.loop = true;
            musicSource.Play();
        }

        // Fade in panel saat masuk Game Over
        StartCoroutine(FadeInPanel());
    }

    IEnumerator FadeInPanel()
    {
        gameOverPanel.alpha = 0;
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            gameOverPanel.alpha = Mathf.Lerp(0, 1, t / fadeDuration);
            yield return null;
        }
        gameOverPanel.alpha = 1;
    }

    public void PlayClickSound()
    {
        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }

    public void RestartGame()
    {
        PlayClickSound();
        Time.timeScale = 1;
        SceneManager.LoadScene("PreGameSceneLv4"); 
    }

    public void BackToMenu()
    {
        PlayClickSound();
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
}
