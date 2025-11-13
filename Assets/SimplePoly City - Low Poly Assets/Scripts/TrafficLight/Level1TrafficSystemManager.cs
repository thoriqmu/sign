using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1TrafficSystemManager : MonoBehaviour
{
    [Header("Group A (arah kanan)")]
    public List<TrafficLightController> groupA = new List<TrafficLightController>();

    [Header("Group B (arah kiri)")]
    public List<TrafficLightController> groupB = new List<TrafficLightController>();

    [Header("Timing (detik)")]
    public float greenTime = 6f;
    public float yellowTime = 2f;
    public float allRedTime = 1.5f;

    void Start()
    {
        StartCoroutine(ControlTraffic());
    }

    IEnumerator ControlTraffic()
    {
        while (true)
        {
            // === Grup A Nyala ===
            SetGroup(groupA, false, false, true); // hijau
            SetGroup(groupB, true, false, false); // merah
            yield return new WaitForSeconds(greenTime);

            SetGroup(groupA, false, true, false); // kuning
            yield return new WaitForSeconds(yellowTime);

            SetGroup(groupA, true, false, false); // merah semua
            yield return new WaitForSeconds(allRedTime);

            // === Grup B Nyala ===
            SetGroup(groupB, false, false, true); // hijau
            SetGroup(groupA, true, false, false); // merah
            yield return new WaitForSeconds(greenTime);

            SetGroup(groupB, false, true, false); // kuning
            yield return new WaitForSeconds(yellowTime);

            SetGroup(groupB, true, false, false); // merah semua
            yield return new WaitForSeconds(allRedTime);
        }
    }

    void SetGroup(List<TrafficLightController> group, bool red, bool yellow, bool green)
    {
        foreach (var light in group)
        {
            if (light != null)
                light.SetLight(red, yellow, green);
        }
    }
}
