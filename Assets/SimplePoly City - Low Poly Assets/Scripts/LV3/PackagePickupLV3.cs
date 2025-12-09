using UnityEngine;

public class PackagePickupLv3 : MonoBehaviour
{
    public AudioClip pickupSound;      // suara yang mau diputar

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);
            HudManagerLv3.Instance.AddPackage();
            Destroy(gameObject);
        }
    }
}
