using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance;

    [Header("UI References")]
    public TMP_Text timeText;
    public TMP_Text packageText;
    public TMP_Text speedText;

    [Header("Stars UI")]
    public Image[] stars; // 3 Image bintang

    [Header("Game Settings")]
    public int totalPackages = 5;

    [HideInInspector]
    public float timer = 0f;
    [HideInInspector]
    public int collectedPackages = 0;
    [HideInInspector]
    public bool allPackagesCollected = false;

    private bool finished = false;
    private Controller playerController;

    void Awake() => Instance = this;

    void Start()
    {
        playerController = Controller.Instance;
        if (playerController == null)
            Debug.LogWarning("HUDManager: Controller.Instance belum ditemukan!");
    }

    void Update()
    {
        if (finished) return;

        // Update timer
        timer += Time.deltaTime;
        UpdateTimeUI();

        // Update paket
        UpdatePackageUI();

        // Update speed
        UpdateSpeedUI();

        // Update bintang
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

        if (timer < 180) starCount = 3;       // < 3 menit
        else if (timer < 300) starCount = 2;  // 3-5 menit
        else if (timer > 420) starCount = 1;  // 5-7 menit
        else starCount = 0;                   // > 7 menit

        for (int i = 0; i < stars.Length; i++)
        {
            if (stars[i] != null)
                stars[i].enabled = i < starCount;
        }
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

    public void FinishGame()
    {
        finished = true;

        int starCount = 0;
        for (int i = 0; i < stars.Length; i++)
            if (stars[i] != null && stars[i].enabled)
                starCount++;

        string starStr = new string('★', starCount) + new string('☆', stars.Length - starCount);
        if (PopupManager.Instance != null)
            PopupManager.Instance.ShowPopup($"FINISH! ");
    }
}
