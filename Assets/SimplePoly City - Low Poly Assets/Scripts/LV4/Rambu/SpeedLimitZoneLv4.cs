using UnityEngine;
using System.Collections;

public class SpeedLimitZoneLv4 : MonoBehaviour
{
    [Header("Speed Limit Settings")]
    public float speedLimitKmh = 30f;   // Batas kecepatan lebih kecil dari controller
    public float penaltyTime = 10f;     // Penalti waktu ketika melanggar
    public float checkInterval = 0.3f;  // Cek kecepatan tiap 0.3 detik
    public bool giveOnce = false;       // Kalau true → hanya 1 kali penalti

    private bool playerInside = false;
    private bool alreadyPunished = false;

    [Header("Audio Warning")]
    public AudioSource audioSource;
    public AudioClip warningSound;

    private ControllerLv4 player => ControllerLv4.Instance;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = true;

        // Mulai cek speed hanya jika belum pernah dihukum (kalau giveOnce aktif)
        if (!alreadyPunished)
            StartCoroutine(CheckSpeedRoutine());
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInside = false;
    }

    IEnumerator CheckSpeedRoutine()
    {
        while (playerInside)
        {
            if (player == null)
                yield break;

            float speedKmh = player.GetCurrentSpeed() * 3.6f;

            if (speedKmh > speedLimitKmh)
            {
                // ======== Tambah waktu (10 detik) ========
                HudManagerLv4.Instance.AddPenalty(
                    penaltyTime,
                    $"Melewati batas kecepatan! Maks {speedLimitKmh} km/h"
                );

                // ======== Suara peringatan ========
                if (audioSource != null && warningSound != null)
                    audioSource.PlayOneShot(warningSound);

                // Jika cuma 1x dihukum → selesai
                if (giveOnce)
                {
                    alreadyPunished = true;
                    yield break;
                }
            }

            yield return new WaitForSeconds(checkInterval);
        }
    }
}
