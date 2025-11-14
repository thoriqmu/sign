using UnityEngine;

public class TrafficLightController : MonoBehaviour
{
    [Header("Lamp Objects (Renderer)")]
    public Renderer redRenderer;
    public Renderer yellowRenderer;
    public Renderer greenRenderer;

    [Header("Light Halos (Optional)")]
    public GameObject redHalo;
    public GameObject yellowHalo;
    public GameObject greenHalo;

    [Header("Materials")]
    public Material LightsOnMat;
    public Material LightsOffMat;

    public enum LightState { Red, Yellow, Green }
    [HideInInspector] public LightState currentState = LightState.Red;

    public void SetLight(bool red, bool yellow, bool green)
    {
        // === SIMPAN STATUS SAAT INI ===
        if (red) currentState = LightState.Red;
        else if (yellow) currentState = LightState.Yellow;
        else if (green) currentState = LightState.Green;

        // === UBAH MATERIAL ===
        if (redRenderer)
            redRenderer.material = red ? LightsOnMat : LightsOffMat;
        if (yellowRenderer)
            yellowRenderer.material = yellow ? LightsOnMat : LightsOffMat;
        if (greenRenderer)
            greenRenderer.material = green ? LightsOnMat : LightsOffMat;

        // === AKTIFKAN HALO ===
        if (redHalo) redHalo.SetActive(red);
        if (yellowHalo) yellowHalo.SetActive(yellow);
        if (greenHalo) greenHalo.SetActive(green);
    }

    // Getter status lampu
    public bool IsRed() => currentState == LightState.Red;
    public bool IsGreen() => currentState == LightState.Green;
    public bool IsYellow() => currentState == LightState.Yellow;
}
