using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishTriggerLv3 : MonoBehaviour
{
    [Header("Scene")]
    public string finishSceneName = "FinishSceneLv3";

    [Header("Audio")]
    public AudioSource sfxSource;       // 1 AudioSource untuk SFX
    public AudioClip successSound;      // suara ketika semua paket lengkap
    public AudioClip failSound;         // suara ketika paket belum lengkap

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (HudManagerLv3.Instance.allPackagesCollected)
        {
            // ▶ Play success sound
            if (successSound != null)
                sfxSource.PlayOneShot(successSound);

            HudManagerLv3.Instance.FinishGame();

            // Delay pop up dan masuk scene akhir
            Invoke(nameof(GoToFinishScene), 2f);
        }
        else
        {
            // ▶ Play fail sound
            if (failSound != null)
                sfxSource.PlayOneShot(failSound);

            PopupManager.Instance.ShowPopup("Ambil semua paket dulu!");
        }
    }

    void GoToFinishScene()
    {
        SceneManager.LoadScene(finishSceneName);
    }
}
