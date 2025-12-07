using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class ControllerLv4 : MonoBehaviour
{
    public static ControllerLv4 Instance;

    [Header("Movement")]
    public float speed = 14f;
    public float sprintMultiplier = 1.5f;
    public float rotationSpeed = 120f;

    [Header("Acceleration Settings")]
    public float acceleration = 4f;
    public float deceleration = 6f;

    [Header("Brake Settings")]
    public float brakeDeceleration = 12f;

    [Header("Audio Clips")]
    public AudioClip engineClip;
    public AudioClip brakeClip;
    public AudioClip crashClip;

    [Header("Settings")]
    public float maxEnginePitch = 2f;
    public float minEnginePitch = 0.8f;

    [Header("Wheels (visual)")]
public Transform wheelFL;   // depan kiri
public Transform wheelFR;   // depan kanan
public Transform wheelRL;   // belakang kiri
public Transform wheelRR;   // belakang kanan

[Tooltip("Jari-jari roda dalam meter (ukur kira-kira di Scene)")]
public float wheelRadius = 0.35f;

[Tooltip("Sudut belok maksimal roda depan (derajat)")]
public float maxSteerAngle = 35f;

// simpan rotasi awal biar steering tidak 'nyimpen' rotasi terus
private Quaternion flBaseRot, frBaseRot, rlBaseRot, rrBaseRot;

    private Rigidbody rb;
    private AudioSource engineSource;
    private AudioSource sfxSource;

    private float currentSpeed = 0f;
    private float targetSpeed = 0f;

    // =====================================================
    //  TAMBAHAN FITUR TABRAKAN (PENTING)
    // =====================================================
    [Header("Collision System")]
    public int maxHits = 2;
    private int hitCount = 0;
    private bool hitCooldown = false;
    public string gameOverScene = "GameOverSceneLv4";


    void Awake() => Instance = this;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // 🔥 ANTI TEMBUS
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        // Engine
        engineSource = gameObject.AddComponent<AudioSource>();
        engineSource.clip = engineClip;
        engineSource.loop = true;
        engineSource.spatialBlend = 1f;
        engineSource.Play();

        // SFX
        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.loop = false;
        sfxSource.spatialBlend = 1f;

        if (wheelFL) flBaseRot = wheelFL.localRotation;
if (wheelFR) frBaseRot = wheelFR.localRotation;
if (wheelRL) rlBaseRot = wheelRL.localRotation;
if (wheelRR) rrBaseRot = wheelRR.localRotation;
    }

    void FixedUpdate()
    {
        if (Keyboard.current == null) return;

        float moveVertical = 0f;
        float turn = 0f;

        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) moveVertical = 1f;
        else if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) moveVertical = -1f;

        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) turn = -1f;
        else if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) turn = 1f;

        float finalSpeed = speed;
        if (Keyboard.current.leftShiftKey.isPressed || Keyboard.current.rightShiftKey.isPressed)
            finalSpeed *= sprintMultiplier;

        bool isBraking = Keyboard.current.spaceKey.isPressed;

        if (isBraking)
        {
            currentSpeed = Mathf.MoveTowards(
                currentSpeed,
                0f,
                brakeDeceleration * Time.fixedDeltaTime
            );

            if (!sfxSource.isPlaying && brakeClip != null)
            {
                sfxSource.clip = brakeClip;
                sfxSource.Play();
            }
        }
        else
        {
            targetSpeed = moveVertical * finalSpeed;

            if (moveVertical != 0)
            {
                currentSpeed = Mathf.MoveTowards(
                    currentSpeed,
                    targetSpeed,
                    acceleration * Time.fixedDeltaTime
                );
            }
            else
            {
                currentSpeed = Mathf.MoveTowards(
                    currentSpeed,
                    0f,
                    deceleration * Time.fixedDeltaTime
                );
            }
        }

// --- STEERING (roda depan tetap boleh belok meski diam) ---
float steer = turn * maxSteerAngle;
if (wheelFL) wheelFL.localRotation = flBaseRot * Quaternion.Euler(0f, steer, 0f);
if (wheelFR) wheelFR.localRotation = frBaseRot * Quaternion.Euler(0f, steer, 0f);

// --- SPINNING (roll roda, hanya kalau ada gerak) ---
if (wheelRadius > 0.0001f)
{
    float angularDeg = (currentSpeed / wheelRadius) * Mathf.Rad2Deg * Time.fixedDeltaTime;
    if (wheelFL) wheelFL.Rotate(Vector3.right, angularDeg, Space.Self);
    if (wheelFR) wheelFR.Rotate(Vector3.right, angularDeg, Space.Self);
    if (wheelRL) wheelRL.Rotate(Vector3.right, angularDeg, Space.Self);
    if (wheelRR) wheelRR.Rotate(Vector3.right, angularDeg, Space.Self);
}

// --- GERAK & ROTASI BODI ---
// mobil hanya berotasi/bergerak jika ada kecepatan
bool isMoving = Mathf.Abs(currentSpeed) > 0.01f;

if (isMoving)
{
    // maju/mundur pakai velocity
    rb.linearVelocity = transform.forward * currentSpeed;

    // opsional: feel mobil nyata → saat mundur arah rotasi dibalik
    float dir = Mathf.Sign(currentSpeed); // +1 maju, -1 mundur
    float rotation = turn * rotationSpeed * dir * Time.fixedDeltaTime;
    rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, rotation, 0f));
}
else
{
    // diam total (A/D hanya memutar roda depan)
    rb.linearVelocity = Vector3.zero;
    // jangan ubah rotasi bodi saat diam
}


        // Pitch sound
        float speedPercent = Mathf.Abs(currentSpeed) / finalSpeed;
        engineSource.pitch = Mathf.Lerp(minEnginePitch, maxEnginePitch, speedPercent);
    }

    // =====================================================
    //               SISTEM TABRAKAN LENGKAP
    // =====================================================
        void OnCollisionEnter(Collision collision)
    {
        // ========= 1. SUARA TABRAKAN — SELALU BUNYI =========
        float speed = rb.linearVelocity.magnitude;
        float strength = Mathf.Clamp01(speed / 20f);

        float volume = Mathf.Lerp(0.3f, 1f, strength);
        float pitch = Mathf.Lerp(0.8f, 1.2f, strength);

        sfxSource.pitch = pitch;
        sfxSource.PlayOneShot(crashClip, volume);   // ALWAYS sound


        // ========= 2. CEK NPC UNTUK DAMAGE =========
        NPCCarLv4 npc = collision.collider.GetComponent<NPCCarLv4>();
        if (npc == null)
            return;  // kalau bukan NPC, selesai sampai di sini (tidak tambah hit)


        // ========= 3. NPC → DAMAGE + POPUP + GAMEOVER =========
        if (hitCooldown) return;
        hitCooldown = true;
        StartCoroutine(HitCooldownRoutine());

        hitCount++;

        // POPUP WARNING
        if (hitCount < maxHits)
        {
            PopupManager.Instance?.ShowWarning($"PERINGATAN! ({hitCount}/{maxHits})");
            return;
        }

        // GAME OVER
        StartCoroutine(GameOverRoutine());
    }

    IEnumerator HitCooldownRoutine()
    {
        yield return new WaitForSeconds(3.5f);
        hitCooldown = false;
    }

    IEnumerator GameOverRoutine()
    {
        // Slow motion sedikit
        Time.timeScale = 0.20f;
        yield return new WaitForSecondsRealtime(3.5f);
        Time.timeScale = 1f;

        SceneManager.LoadScene(gameOverScene);
    }

    public float GetCurrentSpeed()
    {
        return Mathf.Abs(currentSpeed);
    }
}
