using UnityEngine;

public class NPC : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 8f;          // kecepatan NPC
    public float turnSpeed = 50f;     // kecepatan belok
    public Transform[] waypoints;     // titik-titik jalur
    private int currentWaypoint = 0;

    private Rigidbody rb;
    private Renderer rend;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        rend = GetComponent<Renderer>();
    }

    void FixedUpdate()
    {
        if (waypoints.Length == 0) return;

        // Hanya bergerak jika terlihat kamera
        if (!rend.isVisible) return;

        // Jika sudah sampai waypoint terakhir → berhenti
        if (currentWaypoint >= waypoints.Length) return;

        Transform target = waypoints[currentWaypoint];

        // arah ke waypoint
        Vector3 direction = (target.position - transform.position).normalized;

        // rotasi halus ke target
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, lookRotation, turnSpeed * Time.fixedDeltaTime));

        // maju
        rb.MovePosition(rb.position + transform.forward * speed * Time.fixedDeltaTime);

        // cek jarak ke waypoint
        if (Vector3.Distance(transform.position, target.position) < 1f)
        {
            currentWaypoint++;
        }
    }
}
