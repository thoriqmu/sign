using UnityEngine;

[RequireComponent(typeof(Collider))]
public class MandatoryTurnSign : MonoBehaviour
{
    public enum RequiredTurn { Straight, Left, Right }

    public RequiredTurn mustDo = RequiredTurn.Right;
    public float penaltySeconds = 10f;

    [Header("Approach filter")]
    public Transform forwardRef;                // arah "benar" dari pendekatan (point TO intersection)
    [Range(0f,1f)] public float approachDotThreshold = 0.7f;

    [Header("Timing / tolerance")]
    public float minSpeed = 0.4f;
    public float enterGrace = 0.08f;
    public float straightAngleTolerance = 30f;
    public float turnAngleMin = 35f;
    public float minExitDistance = 1.0f;       // minimal jarak dari center untuk hitung exit

    [Header("Audio (optional)")]
    public AudioSource audioSource;
    public AudioClip penaltyClip;

    // internal
    private GameObject playerGO;
    private Vector3 entryDir;   // direction from entry position toward trigger center (horizontal)
    private float enterTime;
    private Vector3 entryPos;

    void Reset()
    {
        var col = GetComponent<Collider>();
        if (col) col.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // simpan entry pos & time
        playerGO = other.gameObject;
        enterTime = Time.time;
        entryPos = other.transform.position;

        // compute entryDir: vector from player -> trigger center (points into trigger)
        Vector3 toCenter = (transform.position - entryPos);
        toCenter.y = 0f;
        if (toCenter.sqrMagnitude < 0.0001f) return;
        entryDir = toCenter.normalized;

        // Jika forwardRef diisi, pastikan entry datang dari arah yang sesuai
        if (forwardRef != null)
        {
            Vector3 refFwd = GetHorizontalForward(forwardRef); // points TO intersection
            float dot = Vector3.Dot(entryDir, refFwd); // +1 = datang sepanjang refFwd
            if (dot < approachDotThreshold)
            {
                // masuk dari sisi lain -> ignore this trigger by clearing playerGO
                playerGO = null;
                return;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // hanya tangani jika sama player yang masuk
        if (playerGO == null || other.gameObject != playerGO) return;

        // jangan langsung hitung kalau keluar terlalu cepat (enterGrace)
        if (Time.time - enterTime < enterGrace)
        {
            Cleanup();
            return;
        }

        // ambil exit pos & kecepatan
        Vector3 exitPos = other.transform.position;
        Rigidbody rb = other.attachedRigidbody;
        float playerSpeed = rb != null ? rb.linearVelocity.magnitude : 0f;

        // cek exit pos jauh dari center supaya benar-benar sudah melewati
        float distFromCenter = Vector3.Distance(new Vector3(exitPos.x,0,exitPos.z), new Vector3(transform.position.x,0,transform.position.z));
        if (distFromCenter < minExitDistance)
        {
            // masih di area tengah -> abaikan
            Cleanup();
            return;
        }

        // jika speed terlalu kecil dan durasi singkat -> abaikan (manuver)
        if (playerSpeed < minSpeed && Time.time - enterTime < 1.0f)
        {
            Cleanup();
            return;
        }

        // compute entryDir (already stored) and exitDir (vector from trigger center to exit)
        Vector3 exitDir = (new Vector3(exitPos.x,0,exitPos.z) - new Vector3(transform.position.x,0,transform.position.z)).normalized;

        float angle = Vector3.SignedAngle(entryDir, exitDir, Vector3.up); // negative left, positive right
        float absAngle = Mathf.Abs(angle);

        bool didStraight = absAngle <= straightAngleTolerance;
        bool didTurnRight = angle > turnAngleMin;
        bool didTurnLeft  = angle < -turnAngleMin;

        bool ok = false;
        switch (mustDo)
        {
            case RequiredTurn.Straight: ok = didStraight; break;
            case RequiredTurn.Left:    ok = didTurnLeft; break;
            case RequiredTurn.Right:   ok = didTurnRight; break;
        }

        if (!ok)
        {
            if (HudManagerLv4.Instance != null)
                HudManagerLv4.Instance.AddPenalty(penaltySeconds, $"Melanggar rambu wajib {mustDo}!");

            if (audioSource != null && penaltyClip != null)
                audioSource.PlayOneShot(penaltyClip);
        }

        Cleanup();
    }

    void Cleanup()
    {
        playerGO = null;
        entryDir = Vector3.zero;
        entryPos = Vector3.zero;
    }

    Vector3 GetHorizontalForward(Transform t)
    {
        Vector3 f = t.forward;
        f.y = 0f;
        if (f.sqrMagnitude < 0.0001f) return Vector3.forward;
        return f.normalized;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        var col = GetComponent<Collider>();
        if (col) Gizmos.DrawWireCube(col.bounds.center, col.bounds.size);

        if (forwardRef != null)
        {
            Gizmos.color = Color.green;
            Vector3 p = forwardRef.position;
            Gizmos.DrawLine(p, p + GetHorizontalForward(forwardRef) * 2f);
        }
    }
}
