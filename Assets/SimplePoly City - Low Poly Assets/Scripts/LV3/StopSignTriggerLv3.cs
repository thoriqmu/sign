using UnityEngine;

public class StopSignTriggerLv3 : MonoBehaviour
{
    [Header("Settings")]
    public float requiredStopTime = 3f;   // harus berhenti 3 detik
    public float penaltyTime = 6f;        // hukuman
    public float stopSpeedThreshold = 0.2f; // batas dianggap berhenti

    private ControllerLv3 playerController;
    private float stopTimer = 0f;
    private bool playerInside = false;
    private bool penaltyGiven = false;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip sidewalkSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            penaltyGiven = false;
            stopTimer = 0f;

            playerController = other.GetComponent<ControllerLv3>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;

            if (!penaltyGiven && stopTimer < requiredStopTime)
            {
                HudManagerLv3.Instance.AddPenalty(penaltyTime, "Tidak berhenti 3 detik di rambu STOP!");
                if (audioSource != null && sidewalkSound != null)
                    audioSource.PlayOneShot(sidewalkSound);
            }
        }
    }

    private void Update()
    {
        if (!playerInside || playerController == null) return;

        float speed = playerController.GetCurrentSpeed();

        if (speed < stopSpeedThreshold)
        {
            stopTimer += Time.deltaTime;

            if (stopTimer >= requiredStopTime && !penaltyGiven)
            {
                penaltyGiven = true; // berhasil berhenti → tidak kena hukuman
            }
        }
        else
        {
            stopTimer = 0f; // bergerak lagi → hitungan ulang
        }
    }
}
