using UnityEngine;

public class BarrierUnlocker : MonoBehaviour
{
    [Header("Barrier Settings")]
    public GameObject[] barriers;   // isi dengan 3 tembok di inspector
    public bool destroyInsteadOfDisable = false;

    private bool unlocked = false;

    void Update()
    {
        // Cek apakah paket sudah lengkap
        if (!unlocked && HudManagerLv4.Instance != null && HudManagerLv4.Instance.allPackagesCollected)
        {
            UnlockBarrier();
        }
    }

    void UnlockBarrier()
    {
        unlocked = true;

        foreach (GameObject b in barriers)
        {
            if (b != null)
            {
                if (destroyInsteadOfDisable)
                    Destroy(b);           // hilang selamanya
                else
                    b.SetActive(false);   // hanya dimatikan
            }
        }

        Debug.Log("🚧 Barrier terbuka! Semua paket sudah terkumpul.");
    }
}
