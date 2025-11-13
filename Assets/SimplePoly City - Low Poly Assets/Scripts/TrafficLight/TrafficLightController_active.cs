// using UnityEngine;
// using System.Collections;

// public class TrafficLightController : MonoBehaviour
// {
//     [Header("Lamp Objects (Renderer)")]
//     public Renderer redRenderer;
//     public Renderer yellowRenderer;
//     public Renderer greenRenderer;

//     [Header("Light Halos (Optional)")]
//     public GameObject redHalo;
//     public GameObject yellowHalo;
//     public GameObject greenHalo;

//     [Header("Materials")]
//     public Material LightsOnMat;
//     public Material LightsOffMat;

//     [Header("Timing (seconds)")]
//     public float redDuration = 5f;
//     public float yellowDuration = 2f;
//     public float greenDuration = 5f;
//     public float allRedDelay = 1f; // jeda antar arah, biar realistis

//     private enum LightState { Red, Yellow, Green }
//     private LightState currentState;

//     void Start()
//     {
//         StartCoroutine(TrafficCycle());
//     }

//     IEnumerator TrafficCycle()
//     {
//         while (true)
//         {
//             // 🔴 RED
//             SetLight(true, false, false);
//             yield return new WaitForSeconds(redDuration);

//             // 🟡 YELLOW
//             SetLight(false, true, false);
//             yield return new WaitForSeconds(yellowDuration);

//             // 🔁 ALL RED DELAY
//             SetLight(true, false, false);
//             yield return new WaitForSeconds(allRedDelay);

//             // 🟢 GREEN
//             SetLight(false, false, true);
//             yield return new WaitForSeconds(greenDuration);

//             // ⏱️ Kembali ke merah
//             SetLight(false, true, false);
//             yield return new WaitForSeconds(yellowDuration);

//             SetLight(true, false, false);
//             yield return new WaitForSeconds(allRedDelay);
//         }
//     }

//     void SetLight(bool red, bool yellow, bool green)
//     {
//         if (redRenderer)
//             redRenderer.material = red ? LightsOnMat : LightsOffMat;
//         if (yellowRenderer)
//             yellowRenderer.material = yellow ? LightsOnMat : LightsOffMat;
//         if (greenRenderer)
//             greenRenderer.material = green ? LightsOnMat : LightsOffMat;

//         if (redHalo) redHalo.SetActive(red);
//         if (yellowHalo) yellowHalo.SetActive(yellow);
//         if (greenHalo) greenHalo.SetActive(green);
//     }
// }
