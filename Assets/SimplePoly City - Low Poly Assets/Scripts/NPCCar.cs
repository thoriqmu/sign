using UnityEngine;

public class NPCCar : MonoBehaviour
{
    public float maxSpeed = 10f;
    public float currentSpeed;
    public float safeDistance = 10f;
    public float brakeStrength = 3f;
    [HideInInspector] public WaypointManager waypointManager;
    [HideInInspector] public CarSpawner spawner;   // <<— Tambahan

    private int currentIndex = 0;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (maxSpeed <= 0) maxSpeed = 8f;
        currentSpeed = maxSpeed;
    }

    void Update()
    {
        if (waypointManager == null || waypointManager.waypoints.Length == 0)
        {
            Debug.LogWarning($"{gameObject.name} tidak punya waypoint!");
            return;
        }

        Transform target = waypointManager.waypoints[currentIndex];
        Vector3 dir = (target.position - transform.position).normalized;

        // sensor ke depan
        float desiredSpeed = maxSpeed;
        if (Physics.Raycast(transform.position + Vector3.up * 0.5f + transform.forward * 1.5f,
                            transform.forward, out RaycastHit hit, safeDistance))
        {
            var otherNpc = hit.collider.GetComponent<NPCCar>();
            if (otherNpc != null)
            {
                desiredSpeed = Mathf.Min(desiredSpeed, otherNpc.currentSpeed * 0.9f);
            }

            var player = hit.collider.GetComponent<Controller>();
            if (player != null)
            {
                desiredSpeed = Mathf.Min(desiredSpeed, player.GetCurrentSpeed() * 0.9f);
            }

            float distanceFactor = Mathf.Clamp01(hit.distance / safeDistance);
            desiredSpeed *= distanceFactor;
        }

        // Haluskan perubahan kecepatan
        currentSpeed = Mathf.MoveTowards(currentSpeed, desiredSpeed, brakeStrength * Time.deltaTime);

        // Gerak
        transform.position += dir * currentSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 2f);

        // Ganti waypoint
        if (Vector3.Distance(transform.position, target.position) < 1f)
        {
            currentIndex++;
            if (currentIndex >= waypointManager.waypoints.Length)
            {
                // kasih tahu spawner untuk kurangi jumlah
                if (spawner != null)
                {
                    spawner.NotifyCarDestroyed();
                }
                Destroy(gameObject);
            }
        }
    }
}
