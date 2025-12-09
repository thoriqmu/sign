using UnityEngine;

public class TrotoarTriggerLv3 : MonoBehaviour
{
    [Header("Penalty Settings")]
    public float penaltyTime = 12f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip sidewalkSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HudManagerLv3.Instance.AddPenalty(penaltyTime, "Hindari Lewat Trotoar!");

            if (audioSource != null && sidewalkSound != null)
                audioSource.PlayOneShot(sidewalkSound);
        }
    }
}
