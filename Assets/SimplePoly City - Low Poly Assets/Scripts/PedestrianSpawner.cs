using UnityEngine;

public class PedestrianSpawner : MonoBehaviour
{
    [System.Serializable]
    public class Route
    {
        public WaypointManager waypointManager;
        public Transform spawnPoint;
    }

    [Header("Daftar Rute Pejalan")]
    public Route[] routes;

    [Header("Prefab Pejalan")]
    public GameObject[] pedestrianPrefabs;
    public float spawnInterval = 4f;
    public int maxPedestrians = 10;

    private int activeCount = 0;

    void Start()
    {
        InvokeRepeating(nameof(SpawnPedestrian), 1f, spawnInterval);
    }

    void SpawnPedestrian()
    {
        if (activeCount >= maxPedestrians) return;

        int r = Random.Range(0, routes.Length);
        Route selectedRoute = routes[r];
        if (selectedRoute.waypointManager == null || selectedRoute.spawnPoint == null)
        {
            Debug.LogWarning("Route pejalan belum lengkap di index " + r);
            return;
        }

        int randPrefab = Random.Range(0, pedestrianPrefabs.Length);
        Transform start = selectedRoute.waypointManager.waypoints[0];
        GameObject ped = Instantiate(
            pedestrianPrefabs[randPrefab],
            start.position,
            start.rotation
        );

        PedestrianNPC npc = ped.GetComponent<PedestrianNPC>();
        npc.waypointManager = selectedRoute.waypointManager;
        npc.spawner = this;

        activeCount++;
        // opsional, auto destroy sudah di NPC saat sampai akhir
        Destroy(ped, 120f);
    }

    public void NotifyPedestrianFinished(PedestrianNPC npc)
    {
        activeCount = Mathf.Max(0, activeCount - 1);
    }
}
