using UnityEngine;

public class RedLightTrigger : MonoBehaviour
{
    [Header("Penalty Settings")]
    public float penaltyTime = 15f;

    [Header("Audio")]
    public AudioSource audioSource;   
    public AudioClip penaltySound;      

    private bool penalized = false;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !penalized)
        {
            TrafficLightTrigger light = GetComponent<TrafficLightTrigger>();

            if (light != null && light.IsRed())
            {
                HUDManager.Instance.AddPenalty(penaltyTime, "Lampu Merah! ");
                penalized = true;

                if (audioSource != null && penaltySound != null)
                    audioSource.PlayOneShot(penaltySound);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            penalized = false;
        }
    }
}
