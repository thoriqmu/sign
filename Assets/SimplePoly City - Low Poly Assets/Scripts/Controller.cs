using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class Controller : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 5f;
    public float rotationSpeed = 120f;

    [Header("Audio Clips")]
    public AudioClip engineClip;
    public AudioClip brakeClip;
    public AudioClip crashClip;

    [Header("Settings")]
    public float maxEnginePitch = 2f;
    public float minEnginePitch = 0.8f;
    public float maxSpeed = 20f;

    private Rigidbody rb;
    private AudioSource engineSource;
    private AudioSource sfxSource;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        // Engine sound
        engineSource = gameObject.AddComponent<AudioSource>();
        engineSource.clip = engineClip;
        engineSource.loop = true;
        engineSource.playOnAwake = false;
        engineSource.spatialBlend = 1f;
        engineSource.Play();

        // SFX (rem, crash)
        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.loop = false;
        sfxSource.playOnAwake = false;
        sfxSource.spatialBlend = 1f;
    }

    void FixedUpdate()
    {
        if (Keyboard.current == null) return;

        float moveVertical = 0f;
        float turn = 0f;

        // Gerak maju/mundur
        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
            moveVertical = 1f;
        else if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)
            moveVertical = -1f;

        // Belok kiri/kanan
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
            turn = -1f;
        else if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
            turn = 1f;

        // REM
        bool isBraking = Keyboard.current.spaceKey.isPressed;
        if (isBraking)
        {
            rb.linearVelocity *= 0.9f; // pelambatan
            // suara rem
            if (!sfxSource.isPlaying)
            {
                sfxSource.clip = brakeClip;
                sfxSource.Play();
            }
        }
        else
        {
            // Gerak maju/mundur
            Vector3 movement = transform.forward * moveVertical * speed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + movement);
        }

        // Rotasi
        float rotation = turn * rotationSpeed * Time.fixedDeltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, rotation, 0f);
        rb.MoveRotation(rb.rotation * turnRotation);

        // Pitch mesin sesuai kecepatan
        float speedPercent = rb.linearVelocity.magnitude / maxSpeed;
        engineSource.pitch = Mathf.Lerp(minEnginePitch, maxEnginePitch, speedPercent);
    }

    void OnCollisionEnter(Collision collision)
    {
        // suara tabrakan
        sfxSource.clip = crashClip;
        sfxSource.Play();
    }

        public float GetCurrentSpeed()
    {
        return rb.linearVelocity.magnitude;
    }
}
