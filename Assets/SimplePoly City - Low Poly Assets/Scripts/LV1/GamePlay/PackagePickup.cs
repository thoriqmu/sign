using UnityEngine;

public class PackagePickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HUDManager.Instance.AddPackage();
            Destroy(gameObject);
        }
    }
}
