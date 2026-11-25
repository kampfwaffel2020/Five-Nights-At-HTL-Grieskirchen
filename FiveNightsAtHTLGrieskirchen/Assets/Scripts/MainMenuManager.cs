using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public Image blackFadeImage;
    public Image newspaperImage;
    public Image nightScreenImage;
    public GameObject staticFlashObject;
    public Sprite[] nightSprites;

    public float fadeDuration = 1f;
    public float newspaperFadeDuration = 1.5f;
    public float newspaperDisplayDuration = 5f;
    public float newspaperFadeOutDuration = 0.8f;
    public float nightFadeInDuration = 0.4f;
    public float nightDisplayDuration = 2.5f;
    public float staticFlashDuration = 0.4f;
    public string gameSceneName = "GameScene";

    private bool isTransitioning;

    public void StartNewGame()
    {
        if (isTransitioning) return;

        PlayerPrefs.SetInt("SavedNight", 1);
        PlayerPrefs.SetInt("CurrentNight", 1);
        PlayerPrefs.Save();

        StartCoroutine(TransitionToGame(1, true));
    }

    public void ContinueGame()
    {
        if (isTransitioning) return;

        int savedNight = PlayerPrefs.GetInt("SavedNight", 1);
        PlayerPrefs.SetInt("CurrentNight", savedNight);
        PlayerPrefs.Save();

        StartCoroutine(TransitionToGame(savedNight, false));
    }

    private IEnumerator TransitionToGame(int nightNumber, bool showNewspaper)
    {
        isTransitioning = true;

        Image staticImg = staticFlashObject != null ? staticFlashObject.GetComponent<Image>() : null;
        float originalStaticAlpha = staticImg != null ? staticImg.color.a : 1f;
        bool staticActive = false;

        if (showNewspaper && newspaperImage != null)
        {
            if (blackFadeImage != null)
            {
                blackFadeImage.gameObject.SetActive(true);
                Color bc = blackFadeImage.color;
                bc.a = 0f;
                blackFadeImage.color = bc;
            }

            newspaperImage.gameObject.SetActive(true);
            Color nc = newspaperImage.color;
            nc.a = 0f;
            newspaperImage.color = nc;

            float elapsed = 0f;
            while (elapsed < newspaperFadeDuration)
            {
                elapsed += Time.deltaTime;
                float alpha = Mathf.Clamp01(elapsed / newspaperFadeDuration);

                nc.a = alpha;
                newspaperImage.color = nc;

                if (blackFadeImage != null)
                {
                    Color bc = blackFadeImage.color;
                    bc.a = alpha;
                    blackFadeImage.color = bc;
                }

                yield return null;
            }

            yield return new WaitForSeconds(newspaperDisplayDuration);

            elapsed = 0f;
            while (elapsed < newspaperFadeOutDuration)
            {
                elapsed += Time.deltaTime;
                nc.a = 1f - Mathf.Clamp01(elapsed / newspaperFadeOutDuration);
                newspaperImage.color = nc;
                yield return null;
            }

            newspaperImage.gameObject.SetActive(false);

            if (staticFlashObject != null)
            {
                staticFlashObject.SetActive(true);
                staticActive = true;
                yield return new WaitForSeconds(staticFlashDuration);
            }
        }
        else if (blackFadeImage != null)
        {
            blackFadeImage.gameObject.SetActive(true);
            Color c = blackFadeImage.color;
            c.a = 0f;
            blackFadeImage.color = c;

            float elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                c.a = Mathf.Clamp01(elapsed / fadeDuration);
                blackFadeImage.color = c;
                yield return null;
            }
        }

        if (nightScreenImage != null && nightSprites != null && nightSprites.Length > 0)
        {
            int index = Mathf.Clamp(nightNumber - 1, 0, nightSprites.Length - 1);
            nightScreenImage.sprite = nightSprites[index];
            Color nightCol = nightScreenImage.color;
            nightCol.a = 0f;
            nightScreenImage.color = nightCol;
            nightScreenImage.gameObject.SetActive(true);

            float elapsed = 0f;
            while (elapsed < nightFadeInDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / nightFadeInDuration);

                nightCol.a = t;
                nightScreenImage.color = nightCol;

                if (staticActive && staticImg != null)
                {
                    Color sc = staticImg.color;
                    sc.a = Mathf.Lerp(originalStaticAlpha, 0f, t);
                    staticImg.color = sc;
                }

                yield return null;
            }
        }

        if (staticActive && staticFlashObject != null)
        {
            staticFlashObject.SetActive(false);
            if (staticImg != null)
            {
                Color sc = staticImg.color;
                sc.a = originalStaticAlpha;
                staticImg.color = sc;
            }
        }

        yield return new WaitForSeconds(nightDisplayDuration);

        if (nightScreenImage != null)
        {
            nightScreenImage.gameObject.SetActive(false);
        }

        if (staticFlashObject != null)
        {
            staticFlashObject.SetActive(true);
        }

        yield return new WaitForSeconds(staticFlashDuration);

        SceneManager.LoadScene(gameSceneName);
    }
}