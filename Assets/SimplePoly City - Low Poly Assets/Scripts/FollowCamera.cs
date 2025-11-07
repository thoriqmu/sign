using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target; 
    public Vector3 offset = new Vector3(0f, 4f, -8f); 
    public float smoothTime = 0.1f; // makin kecil = makin cepat nempel
    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (target == null) return;

        // posisi ideal di belakang mobil (mengikuti arah mobil)
        Vector3 desiredPosition = target.position + target.rotation * offset;

        // pergerakan super halus
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);

        // kamera selalu menghadap mobil
        transform.LookAt(target);
    }
}
