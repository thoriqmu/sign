using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class HudManagerLv3 : MonoBehaviour
{
    public static HudManagerLv3 Instance;

    [Header("UI References")]
    public TMP_Text timeText;
    public TMP_Text packageText;
    public TMP_Text speedText;

    [Header("Stars UI")]
    public Image[] stars;

    [Header("Game Settings")]
    public int totalPackages = 5;

    [HideInInspector] public float timer = 0f;
    [HideInInspector] public int collectedPackages = 0;
    [HideInInspector] public bool allPackagesCollected = false;

    private bool finished = false;
    private ControllerLv3 playerController;

    [Header("Sound Settings")]
    public AudioSource bgmSource;
    public AudioClip backgroundMusic;

    [Range(0f, 3f)] public float fadeDuration = 1.5f;

    void Awake() => Instance = this;

    void Start()
    {
        playerController = ControllerLv3.Instance;

        // Play BGM fade-in
        if (bgmSource != null && backgroundMusic != null)
        {
            bgmSource.clip = backgroundMusic;
            bgmSource.loop = true;
            StartCoroutine(FadeInMusic());
        }
    }

    void Update()
    {
        if (finished) return;

        timer += Time.deltaTime;
        UpdateTimeUI();
        UpdatePackageUI();
        UpdateSpeedUI();
        UpdateStars();
    }

    void UpdateTimeUI()
    {
        int m = Mathf.FloorToInt(timer / 60);
        int s = Mathf.FloorToInt(timer % 60);
        if (timeText != null)
            timeText.text = $": {m:00}:{s:00}";
    }

    void UpdatePackageUI()
    {
        if (packageText != null)
            packageText.text = $": {collectedPackages}/{totalPackages}";
    }

    void UpdateSpeedUI()
    {
        if (playerController != null && speedText != null)
        {
            float speedKmh = playerController.GetCurrentSpeed() * 3.6f;
            speedText.text = $"{speedKmh:0} km/h";
        }
    }

    void UpdateStars()
    {
        int starCount = 3;

        if (timer < 180) starCount = 3;
        else if (timer < 300) starCount = 2;
        else if (timer < 1000) starCount = 1;
        else starCount = 0;

        for (int i = 0; i < stars.Length; i++)
            if (stars[i] != null)
                stars[i].enabled = i < starCount;
    }

    public void AddPackage()
    {
        collectedPackages++;
        UpdatePackageUI();

        if (PopupManager.Instance != null)
            PopupManager.Instance.ShowPopup("Paket terambil!");

        if (collectedPackages >= totalPackages)
        {
            allPackagesCollected = true;
            if (PopupManager.Instance != null)
                PopupManager.Instance.ShowPopup("Semua Paket Terkumpul!");
        }
    }

    public void AddPenalty(float extraTime, string reason)
    {
        timer += extraTime;
        if (PopupManager.Instance != null)
            PopupManager.Instance.ShowPopup($"{reason} (+{extraTime:F1}s)");
    }

    // ================================
    //            FINISH GAME
    // ================================
    public void FinishGame()
    {
        if (finished) return;

        finished = true;

        StartCoroutine(FadeOutMusic());

        int starCount = 0;
        for (int i = 0; i < stars.Length; i++)
            if (stars[i].enabled) starCount++;

        PlayerPrefs.SetFloat("FINAL_TIME", timer);
        PlayerPrefs.SetInt("FINAL_STARS", starCount);
        PlayerPrefs.Save();

        StartCoroutine(LoadFinishSceneAfter(1.5f));
    }

    IEnumerator LoadFinishSceneAfter(float delay)
    {
        yield return new WaitForSeconds(delay);
        UnityEngine.SceneManagement.SceneManager.LoadScene("FinishSceneLv3");
    }

    // ================================
    //            FADE ANIMATION
    // ================================
    IEnumerator FadeInMusic()
    {
        bgmSource.volume = 0f;
        bgmSource.Play();

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            bgmSource.volume = Mathf.Lerp(0f, 0.08f, t / fadeDuration);
            yield return null;
        }
    }

    IEnumerator FadeOutMusic()
    {
        float startVol = bgmSource.volume;

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            bgmSource.volume = Mathf.Lerp(startVol, 0f, t / fadeDuration);
            yield return null;
        }

        bgmSource.Stop();
        bgmSource.volume = startVol;
    }
}
