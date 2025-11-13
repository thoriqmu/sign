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

    public void SetLight(bool red, bool yellow, bool green)
    {
        if (redRenderer)
            redRenderer.material = red ? LightsOnMat : LightsOffMat;
        if (yellowRenderer)
            yellowRenderer.material = yellow ? LightsOnMat : LightsOffMat;
        if (greenRenderer)
            greenRenderer.material = green ? LightsOnMat : LightsOffMat;

        if (redHalo) redHalo.SetActive(red);
        if (yellowHalo) yellowHalo.SetActive(yellow);
        if (greenHalo) greenHalo.SetActive(green);
    }
}
