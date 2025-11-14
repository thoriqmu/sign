using UnityEngine;

public class RedLightTrigger : MonoBehaviour
{
    public float penaltyTime = 15f;
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
