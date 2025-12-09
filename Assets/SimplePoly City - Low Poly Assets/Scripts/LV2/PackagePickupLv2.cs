using UnityEngine;

public class PackagePickupLv2 : MonoBehaviour
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

            HudManagerLv2.Instance.AddPackage();

            Destroy(gameObject);
        }
    }
}
