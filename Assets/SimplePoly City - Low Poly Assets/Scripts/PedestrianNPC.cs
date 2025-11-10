using UnityEngine;

public class PedestrianNPC : MonoBehaviour
{
    public float walkSpeed = 2f;
    public float stopDistance = 0.3f;

    [HideInInspector] public WaypointManager waypointManager;
    [HideInInspector] public PedestrianSpawner spawner; // nanti dipakai untuk notify selesai

    private int currentIndex = 0;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (waypointManager == null || waypointManager.waypoints.Length == 0) return;

        Transform target = waypointManager.waypoints[currentIndex];
        Vector3 dir = (target.position - transform.position);
        float distance = dir.magnitude;

        if (distance > stopDistance)
        {
            dir.Normalize();
            transform.position += dir * walkSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                Quaternion.LookRotation(dir, Vector3.up),
                Time.deltaTime * 5f
            );
            if (animator) animator.SetBool("isWalking", true);
        }
        else
        {
            currentIndex++;
            if (currentIndex >= waypointManager.waypoints.Length)
            {
                // Selesai, laporkan ke spawner lalu hancurkan
                if (spawner != null) spawner.NotifyPedestrianFinished(this);
                Destroy(gameObject);
                return;
            }
        }
    }
}
