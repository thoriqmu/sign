using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishTrigger : MonoBehaviour
{
    public string finishSceneName = "FinishScene";

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (HUDManager.Instance.allPackagesCollected)
        {
            HUDManager.Instance.FinishGame();
            // Delay untuk popup selesai
            Invoke(nameof(GoToFinishScene), 2f);
        }
        else
        {
            PopupManager.Instance.ShowPopup("Ambil semua paket dulu!");
        }
    }

    void GoToFinishScene()
    {
        SceneManager.LoadScene(finishSceneName);
    }
}
