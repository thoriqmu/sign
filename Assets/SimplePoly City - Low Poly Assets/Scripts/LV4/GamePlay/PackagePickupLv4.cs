using UnityEngine;

public class PackagePickupLv4 : MonoBehaviour
{
    public AudioClip pickupSound;
    public GameObject pickupEffect;  // efek visual (particle prefab)

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // mainkan suara
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);

            // spawn efek
            if (pickupEffect != null)
            {
                GameObject vfx = Instantiate(pickupEffect, transform.position, Quaternion.identity);
                Destroy(vfx, 2f); // hapus setelah 2 detik
            }

            HudManagerLv4.Instance.AddPackage();

            Destroy(gameObject);
        }
    }
}
