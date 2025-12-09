using UnityEngine;
using System.Collections.Generic; // Penting untuk List<>

public class NPCCarSpawnerLv1 : MonoBehaviour
{
    [Header("Koleksi Mobil NPC")]
    [Tooltip("Masukkan semua variasi prefab mobil di sini")]
    public GameObject[] npcPrefabs; 

    [Header("Titik Start")]
    public TrafficNodeLv1 startNode;

    [Header("Pengaturan Waktu")]
    public float minSpawnTime = 3f;
    public float maxSpawnTime = 7f;

    private float timer;
    private float nextSpawnTime;

    void Start()
    {
        SetNextSpawnTime();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= nextSpawnTime)
        {
            SpawnCar();
            SetNextSpawnTime();
            timer = 0f;
        }
    }

    void SetNextSpawnTime()
    {
        // Waktu spawn jadi acak, biar tidak kaku seperti robot
        nextSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);
    }

    void SpawnCar()
    {
        if (startNode == null || npcPrefabs.Length == 0) return;

        // 1. Pilih mobil acak
        int randomIndex = Random.Range(0, npcPrefabs.Length);
        GameObject selectedPrefab = npcPrefabs[randomIndex];

        // 2. HITUNG ROTASI OTOMATIS
        // Kita cek dulu target node selanjutnya di mana
        Quaternion spawnRotation = startNode.transform.rotation; // Default ikut rotasi node

        if (startNode.nextNodes.Count > 0 && startNode.nextNodes[0] != null)
        {
            // Hitung arah dari Start Node ke Next Node
            Vector3 direction = startNode.nextNodes[0].transform.position - startNode.transform.position;
            direction.y = 0; // Pastikan mobil tetap datar (tidak nungging)

            if (direction != Vector3.zero)
            {
                // Paksa rotasi menghadap ke target
                spawnRotation = Quaternion.LookRotation(direction);
            }
        }

        // 3. Spawn dengan rotasi yang sudah dihitung (spawnRotation)
        GameObject car = Instantiate(selectedPrefab, startNode.transform.position, spawnRotation);
        
        // 4. Setup Controller
        NPCCarLv1 controller = car.GetComponent<NPCCarLv1>();
        if (controller != null && startNode.nextNodes.Count > 0)
        {
            controller.currentNode = startNode.nextNodes[0];
            
            // Opsional: Langsung set posisi mobil agar menempel sempurna di aspal
            // (Kadang pivot point mobil beda-beda)
            // car.transform.position = new Vector3(car.transform.position.x, startNode.transform.position.y, car.transform.position.z);
        }
    }
}