using UnityEngine;

public class NPCPedestrian : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 1.5f;
    public float runSpeed = 4.0f; // Kecepatan lari
    public float rotationSpeed = 10f;
    public float arrivalDistance = 0.5f;

    [Header("Sensor (Anti Tumpuk)")]
    public float sensorLength = 1.0f;
    public LayerMask pedestrianLayer; // Layer khusus pejalan kaki

    [Header("Animation Settings")]
    public int totalIdleAnimations = 3; // Jumlah variasi animasi idle yang Anda punya

    [Header("References")]
    public Animator animator;
    public PedestrianNode currentNode;

    private bool isWaitingLight = false;
    private bool isBlockedByFriend = false;
    private float currentSpeed;

    void Start()
    {
        if (animator == null) animator = GetComponent<Animator>();
        
        // Random Idle awal
        RandomizeIdle();

        if (currentNode != null)
        {
            transform.position = currentNode.transform.position;
            SetNextDestination();
        }
    }

    void Update()
    {
        // 1. Cek Teman di Depan (Anti Tumpuk)
        CheckObstacle();

        // 2. Logika Berhenti (Karena Lampu atau Teman)
        if (isWaitingLight || isBlockedByFriend)
        {
            // Jika berhenti karena lampu, cek terus status lampunya
            if (isWaitingLight) HandleTrafficLightWait();
            
            // Mainkan animasi Idle
            UpdateAnimation(false, false); 
            return;
        }

        // 3. Jika tidak ada tujuan
        if (currentNode == null) return;

        // 4. Bergerak
        MoveToTarget();
    }

    void CheckObstacle()
    {
        // Tembak laser pendek ke depan
        Ray ray = new Ray(transform.position + Vector3.up * 1f, transform.forward);
        RaycastHit hit;

        // Visualisasi sensor di Scene view
        Debug.DrawRay(ray.origin, ray.direction * sensorLength, Color.red);

        // Jika kena teman (Layer Pedestrian), berhenti
        if (Physics.Raycast(ray, out hit, sensorLength, pedestrianLayer))
        {
            isBlockedByFriend = true;
        }
        else
        {
            isBlockedByFriend = false;
        }
    }

    void MoveToTarget()
    {
        Vector3 direction = currentNode.transform.position - transform.position;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
        }

        // --- LOGIKA LARI VS JALAN ---
        // Jika node tujuan adalah zebra cross, kita LARI
        bool isRunning = currentNode.isZebraCross;
        currentSpeed = isRunning ? runSpeed : walkSpeed;

        transform.position += transform.forward * currentSpeed * Time.deltaTime;

        // Update Animasi (Jalan atau Lari)
        UpdateAnimation(true, isRunning);

        // Cek Sampai
        if (Vector3.Distance(transform.position, currentNode.transform.position) < arrivalDistance)
        {
            CheckNodeRules();
        }
    }

    void CheckNodeRules()
    {
        // Cek Lampu Lalu Lintas
        if (currentNode.trafficLight != null)
        {
            // LOGIKA BARU: Jika Lampu Mobil HIJAU/KUNING -> Kita BERHENTI (Bahaya)
            bool carsAreMoving = currentNode.trafficLight.IsGreen() || currentNode.trafficLight.IsYellow();

            if (carsAreMoving)
            {
                isWaitingLight = true;
                RandomizeIdle(); // Ganti gaya idle saat berhenti menunggu
                return;
            }
        }

        SetNextDestination();
    }

    void HandleTrafficLightWait()
    {
        if (currentNode.trafficLight != null)
        {
            // Jika lampu mobil sudah MERAH -> Kita AMAN JALAN
            if (currentNode.trafficLight.IsRed())
            {
                isWaitingLight = false;
                SetNextDestination();
            }
        }
        else
        {
            isWaitingLight = false; // Error safety
        }
    }

    void SetNextDestination()
    {
        if (currentNode.nextNodes != null && currentNode.nextNodes.Count > 0)
        {
            int index = Random.Range(0, currentNode.nextNodes.Count);
            currentNode = currentNode.nextNodes[index];
        }
        else
        {
            currentNode = null;
            Destroy(gameObject); // Hapus jika buntu
        }
    }

    // Mengacak animasi Idle biar tidak kaku
void RandomizeIdle()
    {
        if (animator)
        {
            // Ambil angka acak (0, 1, atau 2)
            int randomID = Random.Range(0, totalIdleAnimations);
            
            // PENTING: Ganti SetInteger menjadi SetFloat
            // Kita ubah (cast) randomID menjadi float
            animator.SetFloat("IdleIndex", (float)randomID);
        }
    }

    void UpdateAnimation(bool isMoving, bool isRunning)
    {
        if (animator != null)
        {
            animator.SetBool("isWalking", isMoving);
            animator.SetBool("isRunning", isRunning);
        }
    }
}