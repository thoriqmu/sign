using UnityEngine;
using TMPro;
using System.Collections;

public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance;

    public CanvasGroup popupCanvas;
    public TMP_Text popupText;
    public float fadeDuration = 0.5f;
    public float displayDuration = 2f;

    void Awake() => Instance = this;

    public void ShowPopup(string message)
    {
        StopAllCoroutines();
        StartCoroutine(PopupRoutine(message));
    }

    IEnumerator PopupRoutine(string msg)
    {
        popupText.text = msg;

        popupCanvas.alpha = 0;
        popupCanvas.gameObject.SetActive(true); // GameObject tetap aktif

        // Fade In
        float t = 0;
        while (t < fadeDuration)
        {
            popupCanvas.alpha = Mathf.Lerp(0, 1, t / fadeDuration);
            t += Time.deltaTime;
            yield return null;
        }
        popupCanvas.alpha = 1;

        yield return new WaitForSeconds(displayDuration);

        // Fade Out
        t = 0;
        while (t < fadeDuration)
        {
            popupCanvas.alpha = Mathf.Lerp(1, 0, t / fadeDuration);
            t += Time.deltaTime;
            yield return null;
        }

        popupCanvas.alpha = 0;
    }
}
