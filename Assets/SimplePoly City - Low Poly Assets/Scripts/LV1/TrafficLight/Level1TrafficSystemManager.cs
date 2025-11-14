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

    private Coroutine controlRoutine;

    void Start()
    {
        controlRoutine = StartCoroutine(ControlTraffic());
    }

    IEnumerator ControlTraffic()
    {
        while (true)
        {
            // === 1️⃣ Grup A Nyala Hijau ===
            Debug.Log("🚦 FASE 1: GROUP A → HIJAU, GROUP B → MERAH");
            SetGroup(groupA, false, false, true); // hijau
            SetGroup(groupB, true, false, false); // merah
            yield return new WaitForSeconds(greenTime);

            // === 2️⃣ Grup A Kuning ===
            Debug.Log("⚠️ FASE 2: GROUP A → KUNING");
            SetGroup(groupA, false, true, false);
            yield return new WaitForSeconds(yellowTime);

            // === 3️⃣ Semua Merah ===
            Debug.Log("🛑 FASE 3: SEMUA MERAH (CLEAR TIME)");
            SetGroup(groupA, true, false, false);
            SetGroup(groupB, true, false, false);
            yield return new WaitForSeconds(allRedTime);

            // === 4️⃣ Grup B Nyala Hijau ===
            Debug.Log("🚦 FASE 4: GROUP B → HIJAU, GROUP A → MERAH");
            SetGroup(groupB, false, false, true);
            SetGroup(groupA, true, false, false);
            yield return new WaitForSeconds(greenTime);

            // === 5️⃣ Grup B Kuning ===
            Debug.Log("⚠️ FASE 5: GROUP B → KUNING");
            SetGroup(groupB, false, true, false);
            yield return new WaitForSeconds(yellowTime);

            // === 6️⃣ Semua Merah Lagi ===
            Debug.Log("🛑 FASE 6: SEMUA MERAH (CLEAR TIME)");
            SetGroup(groupA, true, false, false);
            SetGroup(groupB, true, false, false);
            yield return new WaitForSeconds(allRedTime);
        }
    }

    void SetGroup(List<TrafficLightController> group, bool red, bool yellow, bool green)
    {
        foreach (var light in group)
        {
            if (light != null)
            {
                light.SetLight(red, yellow, green);

                // Debug warna setiap lampu
                Debug.Log($"[TrafficLight] {light.name}: RED={red}, YELLOW={yellow}, GREEN={green}, State={light.currentState}");
            }
        }
    }
}
