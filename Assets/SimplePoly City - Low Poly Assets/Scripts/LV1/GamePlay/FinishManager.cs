using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class FinishSceneManager : MonoBehaviour
{
    [Header("Story UI")]
    public CanvasGroup storyPanel;
    public TMP_Text storyText;

    [Header("Result UI")]
    public CanvasGroup resultPanel;
    public TMP_Text titleText;
    public TMP_Text timeText;
    public Image[] stars;
    public Sprite starOn;
    public Sprite starOff;

    [Header("Fade Overlay")]
    public Image fadeOverlay;

    [Header("Scenes")]
    public string menuSceneName = "MainMenu";

    void Start()
    {
        // Persiapkan panel
        storyPanel.alpha = 1;
        resultPanel.alpha = 0;
        fadeOverlay.color = new Color(0, 0, 0, 0);

        // Ambil data dari HUDManager
        float finalTime = PlayerPrefs.GetFloat("FINAL_TIME", 0);
        int finalStars = PlayerPrefs.GetInt("FINAL_STARS", 0);

        // Format waktu
        int m = Mathf.FloorToInt(finalTime / 60);
        int s = Mathf.FloorToInt(finalTime % 60);
        timeText.text = $"Waktu: {m:00}:{s:00}";

        // Bintang
        for (int i = 0; i < stars.Length; i++)
            stars[i].sprite = (i < finalStars) ? starOn : starOff;

        // Mulai sequence
        StartCoroutine(FinishSequence());
    }

    IEnumerator FinishSequence()
    {
        // 1. Tampilkan cerita
       storyText.text =
    "Anda telah berhasil mengantarkan seluruh paket dengan aman dan tepat waktu.\n" +
    "Semua kiriman milik Sahroni telah sampai berkat kerja keras dan ketelitian Anda.\n\n" +
    "Terima kasih atas dedikasi Anda — kota dan Sahroni sangat menghargainya.";


        yield return new WaitForSeconds(3f);

        // 2. Fade to black
        yield return StartCoroutine(FadeImage(fadeOverlay, 0, 1, 1f));

        // 3. Sembunyikan panel cerita
        storyPanel.alpha = 0;

        // 4. Tampilkan panel hasil
        yield return StartCoroutine(FadeCanvas(resultPanel, 0, 1, 1.5f));

        // 5. Fade dari black ke normal
        yield return StartCoroutine(FadeImage(fadeOverlay, 1, 0, 1f));
    }

    IEnumerator FadeCanvas(CanvasGroup cg, float from, float to, float duration)
    {
        float t = 0;
        cg.alpha = from;
        while (t < duration)
        {
            t += Time.deltaTime;
            cg.alpha = Mathf.Lerp(from, to, t / duration);
            yield return null;
        }
    }

    IEnumerator FadeImage(Image img, float from, float to, float duration)
    {
        float t = 0;
        Color c = img.color;
        while (t < duration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(from, to, t / duration);
            img.color = c;
            yield return null;
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(menuSceneName);
    }
}
