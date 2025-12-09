using UnityEngine;
using System.Collections;

public class NPCStopLineLv2 : MonoBehaviour
{
    public enum StopType { TrafficLight, StopSign }
    
    [Header("Settings")]
    public StopType stopType;
    
    [Header("Isi jika Traffic Light")]
    public TrafficLightController linkedLight; // Mengambil referensi script traffic light Anda
    
    [Header("Isi jika Stop Sign")]
    public float stopSignDuration = 3f;

    // Logika untuk mengecek apakah NPC boleh jalan
    public bool ShouldStop()
    {
        if (stopType == StopType.TrafficLight && linkedLight != null)
        {
            // Berhenti jika Merah atau Kuning
            return linkedLight.IsRed() || linkedLight.IsYellow();
        }
        return false; // Jika hijau, tidak perlu berhenti (logic stop sign ditangani coroutine di NPC)
    }
}