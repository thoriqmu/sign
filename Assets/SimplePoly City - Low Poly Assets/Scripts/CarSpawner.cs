using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    [System.Serializable]
    public class Route
    {
        public WaypointManager waypointManager;
        public Transform spawnPoint;
    }

    [Header("Daftar Rute")]
    public Route[] routes;

    [Header("Mobil")]
    public GameObject[] carPrefabs;
    public float spawnInterval = 3f;
    public int maxCars = 20;

    private int carCount = 0;

    void Start()
    {
        InvokeRepeating(nameof(SpawnCar), 1f, spawnInterval);
    }

    void SpawnCar()
    {
        if (carCount >= maxCars) return;

        int r = Random.Range(0, routes.Length);
        Route selectedRoute = routes[r];

        if (selectedRoute.waypointManager == null || selectedRoute.spawnPoint == null)
        {
            Debug.LogWarning("❌ Route belum lengkap di index: " + r);
            return;
        }

        // Pilih mobil acak
        int randCar = Random.Range(0, carPrefabs.Length);
        Transform firstWP = selectedRoute.waypointManager.waypoints[0];
        Transform nextWP = null;

        // Cari waypoint berikutnya untuk arah mobil
        if (selectedRoute.waypointManager.waypoints.Length > 1)
            nextWP = selectedRoute.waypointManager.waypoints[1];
        else
            nextWP = firstWP; // fallback kalau cuma satu titik

        // Hitung rotasi awal mobil menghadap waypoint berikutnya
        Quaternion startRotation = Quaternion.LookRotation(
            (nextWP.position - firstWP.position).normalized,
            Vector3.up
        );

        // Spawn mobil di posisi dan arah yang sesuai jalur
        GameObject car = Instantiate(
            carPrefabs[randCar],
            new Vector3(firstWP.position.x, firstWP.position.y, firstWP.position.z),
            startRotation
        );

        // Assign waypoint ke mobil
        NPCCar npc = car.GetComponent<NPCCar>();
        npc.waypointManager = selectedRoute.waypointManager;
        npc.spawner = this;

        carCount++;
        Destroy(car, 60f);
    }

    public void NotifyCarDestroyed()
{
    carCount = Mathf.Max(0, carCount - 1);
}
}
