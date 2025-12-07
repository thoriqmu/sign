using UnityEngine;
using System.Collections; // Wajib untuk IEnumerator

[RequireComponent(typeof(Rigidbody))]
public class NPCCarLv4 : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 10f;
    public float rotationSpeed = 5f;
    public float arrivalDistance = 1.5f;

    [Header("Pathfinding")]
    public TrafficNode currentNode;

    [Header("Sensors")]
    public float sensorLength = 5f;
    public LayerMask obstacleLayer;
    public Vector3 sensorOffset = new Vector3(0, 0.5f, 1f);

    // --- BAGIAN BARU: VISUAL RODA ---
    [Header("Wheel Visuals (Drag dari Hierarchy)")]
    public Transform wheelFL; // Depan Kiri
    public Transform wheelFR; // Depan Kanan
    public Transform wheelRL; // Belakang Kiri
    public Transform wheelRR; // Belakang Kanan
    
    [Tooltip("Jari-jari roda (semakin besar, putaran makin pelan)")]
    public float wheelRadius = 0.35f; 
    public float maxSteerAngle = 30f; // Sudut belok maksimal roda depan
    // --------------------------------

    private Rigidbody rb;
    private bool isStoppedByTraffic = false;
    private bool isStoppedByObstacle = false;

    private Transform meshFL, meshFR, meshRL, meshRR;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        // OTOMATIS MENCARI ANAK (Mesh Roda) DI DALAM HOLDER
        if(wheelFL && wheelFL.childCount > 0) meshFL = wheelFL.GetChild(0);
        if(wheelFR && wheelFR.childCount > 0) meshFR = wheelFR.GetChild(0);
        if(wheelRL && wheelRL.childCount > 0) meshRL = wheelRL.GetChild(0);
        if(wheelRR && wheelRR.childCount > 0) meshRR = wheelRR.GetChild(0);
    }

    void FixedUpdate()
    {
        CheckObstacle();
        MoveCar();
        AnimateWheels(); // Panggil fungsi animasi roda
    }

    void MoveCar()
    {
        if (isStoppedByObstacle || isStoppedByTraffic || currentNode == null)
        {
            rb.linearVelocity = Vector3.zero;
            return;
        }

        // 1. ROTASI BODY MOBIL
        Vector3 direction = currentNode.transform.position - transform.position;
        direction.y = 0; 

        if (direction != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.fixedDeltaTime);
        }

        // 2. MAJU
        Vector3 moveVect = transform.forward * speed;
        moveVect.y = rb.linearVelocity.y;
        rb.linearVelocity = moveVect;

        // 3. CEK JARAK KE NODE
        float distance = Vector3.Distance(transform.position, currentNode.transform.position);
        if (distance < arrivalDistance)
        {
            SetNextNode();
        }
    }

    // --- FUNGSI BARU: ANIMASI RODA ---
    void AnimateWheels()
    {
        // Hitung kecepatan putar
        float currentSpeed = rb.linearVelocity.magnitude;
        float direction = Vector3.Dot(transform.forward, rb.linearVelocity);
        
        // Cek mundur atau maju
        if (direction < -0.1f) currentSpeed = -currentSpeed; // Mundur

        float rotationAngle = (currentSpeed / wheelRadius) * Mathf.Rad2Deg * Time.fixedDeltaTime;

        // 1. SPINNING (Putar ANAKNYA / Mesh) pada sumbu X
        if(meshFL) meshFL.Rotate(Vector3.right, rotationAngle);
        if(meshFR) meshFR.Rotate(Vector3.right, rotationAngle);
        if(meshRL) meshRL.Rotate(Vector3.right, rotationAngle);
        if(meshRR) meshRR.Rotate(Vector3.right, rotationAngle);

        // 2. STEERING (Putar INDUKNYA / Holder) pada sumbu Y
        if (currentNode != null)
        {
            Vector3 targetDir = currentNode.transform.position - transform.position;
            targetDir.y = 0;
            
            float targetSteerAngle = Vector3.SignedAngle(transform.forward, targetDir, Vector3.up);
            targetSteerAngle = Mathf.Clamp(targetSteerAngle, -maxSteerAngle, maxSteerAngle);

            // Kita putar Holder (wheelFL/wheelFR)
            if (wheelFL)
            {
                // Gunakan localRotation murni agar reset ke 0 saat lurus
                Quaternion targetRot = Quaternion.Euler(0, targetSteerAngle, 0);
                wheelFL.localRotation = Quaternion.Slerp(wheelFL.localRotation, targetRot, Time.fixedDeltaTime * 5f);
            }

            if (wheelFR)
            {
                Quaternion targetRot = Quaternion.Euler(0, targetSteerAngle, 0);
                wheelFR.localRotation = Quaternion.Slerp(wheelFR.localRotation, targetRot, Time.fixedDeltaTime * 5f);
            }
        }
    }
    // --------------------------------

    void CheckObstacle()
    {
        Ray ray = new Ray(transform.TransformPoint(sensorOffset), transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, sensorLength, obstacleLayer))
        {
            isStoppedByObstacle = true;
        }
        else
        {
            isStoppedByObstacle = false;
        }
    }

    void SetNextNode()
    {
        if (currentNode.nextNodes == null || currentNode.nextNodes.Count == 0)
        {
            Destroy(gameObject); 
            return;
        }

        int index = Random.Range(0, currentNode.nextNodes.Count);
        currentNode = currentNode.nextNodes[index];
    }

    private void OnTriggerEnter(Collider other)
    {
        NPCStopLine stopLine = other.GetComponent<NPCStopLine>();
        if (stopLine != null)
        {
            if (stopLine.stopType == NPCStopLine.StopType.TrafficLight)
                StartCoroutine(TrafficLightRoutine(stopLine));
            else if (stopLine.stopType == NPCStopLine.StopType.StopSign)
                StartCoroutine(StopSignRoutine(stopLine));
        }
    }

    IEnumerator TrafficLightRoutine(NPCStopLine stopLine)
    {
        while (stopLine.ShouldStop())
        {
            isStoppedByTraffic = true;
            yield return new WaitForSeconds(0.2f);
        }
        isStoppedByTraffic = false;
    }

    IEnumerator StopSignRoutine(NPCStopLine stopLine)
    {
        isStoppedByTraffic = true;
        yield return new WaitForSeconds(stopLine.stopSignDuration);
        isStoppedByTraffic = false;
    }
}