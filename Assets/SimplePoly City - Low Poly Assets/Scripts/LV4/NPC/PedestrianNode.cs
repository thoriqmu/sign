using UnityEngine;
using System.Collections.Generic;

public class PedestrianNode : MonoBehaviour
{
    [Header("Jalur Selanjutnya")]
    public List<PedestrianNode> nextNodes;

    [Header("Aturan")]
    [Tooltip("Isi jika ini titik tunggu sebelum menyeberang")]
    public TrafficLightController trafficLight; 

    [Tooltip("Centang jika jalur menuju node ini adalah Zebra Cross (harus lari)")]
    public bool isZebraCross = false; // <--- VAR BARU

    private void OnDrawGizmos()
    {
        Gizmos.color = isZebraCross ? Color.yellow : Color.blue; // Kuning jika zebra cross
        Gizmos.DrawWireSphere(transform.position, 0.3f);

        if (nextNodes != null)
        {
            Gizmos.color = Color.cyan;
            foreach (var node in nextNodes)
                if (node != null) Gizmos.DrawLine(transform.position, node.transform.position);
        }
    }
}