using UnityEngine;

public class WrongWayTrigger : MonoBehaviour
{
    public float penaltyTime = 10f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HUDManager.Instance.AddPenalty(penaltyTime, "Salah Jalur! ");
        }
    }
}
