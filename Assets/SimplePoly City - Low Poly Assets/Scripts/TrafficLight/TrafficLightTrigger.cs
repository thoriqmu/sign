using UnityEngine;

public class TrafficLightTrigger : MonoBehaviour
{
    public TrafficLightController trafficLight;

    public bool IsRed()
    {
        if (trafficLight == null)
        {
            Debug.LogWarning($"[TrafficLightTrigger] {name}: TrafficLight belum diassign!");
            return false;
        }

        bool merah = trafficLight.IsRed();
        Debug.Log($"[TrafficLightTrigger] {name}: status lampu merah = {merah}");
        return merah;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Collider col = GetComponent<Collider>();
        if (col)
            Gizmos.DrawWireCube(col.bounds.center, col.bounds.size);
    }
}
