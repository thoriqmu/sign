using UnityEngine;
using System.Collections.Generic;

public class TrafficNodeLv3 : MonoBehaviour
{
    [Header("Jalur Selanjutnya")]
    [Tooltip("Tarik TrafficNode lain ke sini. Bisa lebih dari 1 untuk persimpangan.")]
    public List<TrafficNodeLv3> nextNodes;

    [Header("Gizmos (Visual Bantuan)")]
    public float radius = 1f;

    // Menggambar garis di Scene view agar mudah menyusun jalur
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, radius);

        if (nextNodes != null)
        {
            Gizmos.color = Color.yellow;
            foreach (var node in nextNodes)
            {
                if (node != null)
                {
                    Gizmos.DrawLine(transform.position, node.transform.position);
                }
            }
        }
    }
}