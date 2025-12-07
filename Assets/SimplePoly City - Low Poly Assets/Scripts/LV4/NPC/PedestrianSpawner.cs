using UnityEngine;
using System.Collections.Generic;

public class PedestrianSpawner : MonoBehaviour
{
    [Header("Pool Pejalan Kaki")]
    public GameObject[] pedestrianPrefabs; // Daftar prefab (Pria, Wanita, dll)

    [Header("Titik Spawn")]
    public PedestrianNode startNode; // Titik awal di trotoar

    [Header("Pengaturan Spawn")]
    public float minSpawnTime = 5f;
    public float maxSpawnTime = 12f;

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
            SpawnPedestrian();
            SetNextSpawnTime();
            timer = 0f;
        }
    }

    void SetNextSpawnTime()
    {
        nextSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);
    }

    void SpawnPedestrian()
    {
        if (startNode == null || pedestrianPrefabs.Length == 0) return;

        // Pilih prefab acak
        int randomIndex = Random.Range(0, pedestrianPrefabs.Length);
        GameObject prefab = pedestrianPrefabs[randomIndex];

        // Hitung rotasi menghadap node selanjutnya
        Quaternion spawnRotation = transform.rotation;
        if (startNode.nextNodes.Count > 0)
        {
            Vector3 direction = startNode.nextNodes[0].transform.position - startNode.transform.position;
            direction.y = 0;
            if (direction != Vector3.zero) spawnRotation = Quaternion.LookRotation(direction);
        }

        // Munculkan NPC
        GameObject npc = Instantiate(prefab, startNode.transform.position, spawnRotation);
        
        // Hubungkan ke sistem node
        NPCPedestrian controller = npc.GetComponent<NPCPedestrian>();
        if (controller != null)
        {
            controller.currentNode = startNode; // Mengatur titik mulai
        }
    }
}