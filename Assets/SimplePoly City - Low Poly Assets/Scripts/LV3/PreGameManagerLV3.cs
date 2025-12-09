using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PreGameManagerLv3 : MonoBehaviour
{
    [Header("Scene Settings")]
    public string gameplaySceneName = "Level3";
    public string menuSceneName = "MainMenu";
    public string ruleSceneName = "RuleScenesLv3";

    [Header("Audio Settings")]
    public AudioSource musicSource;
    public AudioClip soundtrack;
    public float musicFadeDuration = 1.5f;

    [Header("Button SFX")]
    public AudioSource sfxSource;
    public AudioClip buttonClickSound;

    private void Start()
    {
        if (soundtrack != null)
        {
            musicSource.clip = soundtrack;
            musicSource.volume = 0f;
            musicSource.Play();
            StartCoroutine(FadeAudio(musicSource, 1f, musicFadeDuration));
        }
    }

    public void StartGame()
    {
        PlayButtonSFX();
        StartCoroutine(StartGameSequence());
    }

    public void BackToMenu()
    {
        PlayButtonSFX();
        StartCoroutine(BackToMenuSequence());
    }

    public void RulesMenu()
    {
        PlayButtonSFX();
        StartCoroutine(RuleSequence());
    }

    private System.Collections.IEnumerator FadeAudio(AudioSource source, float targetVolume, float duration)
    {
        float startVolume = source.volume;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            source.volume = Mathf.Lerp(startVolume, targetVolume, time / duration);
            yield return null;
        }

        source.volume = targetVolume;
    }

    private System.Collections.IEnumerator StartGameSequence()
    {
        yield return StartCoroutine(FadeAudio(musicSource, 0f, musicFadeDuration));
        SceneManager.LoadScene(gameplaySceneName);
    }

    private System.Collections.IEnumerator RuleSequence()
    {
        yield return StartCoroutine(FadeAudio(musicSource, 0f, musicFadeDuration));
        SceneManager.LoadScene(ruleSceneName);
    }

    private System.Collections.IEnumerator BackToMenuSequence()
    {
        yield return StartCoroutine(FadeAudio(musicSource, 0f, musicFadeDuration));
        SceneManager.LoadScene(menuSceneName);
    }

    private void PlayButtonSFX()
    {
        if (sfxSource != null && buttonClickSound != null)
        {
            sfxSource.PlayOneShot(buttonClickSound);
        }
    }
}
