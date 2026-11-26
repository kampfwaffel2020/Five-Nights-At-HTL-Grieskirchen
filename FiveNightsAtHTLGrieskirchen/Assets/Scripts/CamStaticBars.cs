using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CamStaticBars : MonoBehaviour
{
    #region Settings
    public int barPoolSize = 10;
    public float randomIntervalMin = 1.2f;
    public float randomIntervalMax = 3.5f;
    public float randomBurstDuration = 0.08f;
    public float switchFlashDuration = 0.22f;
    public float switchStepInterval = 0.04f;
    #endregion

    #region State
    private Image[] barImages;
    private RectTransform containerRect;
    private Coroutine ambientRoutine;
    private Coroutine flashRoutine;
    #endregion

    #region Unity Lifecycle
    private void Awake()
    {
        BuildBarPool();
    }

    private void OnEnable()
    {
        if (barImages == null)
        {
            BuildBarPool();
        }

        HideAllBars();
        ambientRoutine = StartCoroutine(AmbientBarsLoop());
    }

    private void OnDisable()
    {
        if (ambientRoutine != null)
        {
            StopCoroutine(ambientRoutine);
            ambientRoutine = null;
        }

        if (flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
            flashRoutine = null;
        }

        HideAllBars();
    }
    #endregion

    #region Public Actions
    public void TriggerSwitchFlash()
    {
        if (!gameObject.activeInHierarchy) return;

        if (barImages == null)
        {
            BuildBarPool();
        }

        if (flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
        }

        flashRoutine = StartCoroutine(SwitchFlashSequence());
    }
    #endregion

    #region Bar Generation
    private void BuildBarPool()
    {
        containerRect = GetComponent<RectTransform>();
        barImages = new Image[barPoolSize];

        for (int i = 0; i < barPoolSize; i++)
        {
            GameObject barObj = new GameObject("StaticBar_" + i, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            barObj.transform.SetParent(transform, false);

            RectTransform rt = barObj.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0f, 0.5f);
            rt.anchorMax = new Vector2(1f, 0.5f);
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;

            Image img = barObj.GetComponent<Image>();
            img.color = Color.white;
            img.raycastTarget = false;

            barObj.SetActive(false);
            barImages[i] = img;
        }
    }

    private void ShowRandomBars(int count, float minHeight, float maxHeight, float minAlpha, float maxAlpha)
    {
        HideAllBars();

        float areaHeight = containerRect != null && containerRect.rect.height > 0f
            ? containerRect.rect.height
            : 1080f;

        int activeCount = Mathf.Clamp(count, 1, barImages.Length);

        for (int i = 0; i < activeCount; i++)
        {
            Image img = barImages[i];
            RectTransform rt = img.rectTransform;

            float h = Random.Range(minHeight, maxHeight);
            float y = Random.Range(-areaHeight * 0.48f, areaHeight * 0.48f);

            rt.sizeDelta = new Vector2(0f, h);
            rt.anchoredPosition = new Vector2(0f, y);

            Color c = Color.white;
            c.a = Random.Range(minAlpha, maxAlpha);
            img.color = c;

            img.gameObject.SetActive(true);
        }
    }

    private void HideAllBars()
    {
        if (barImages == null) return;

        for (int i = 0; i < barImages.Length; i++)
        {
            if (barImages[i] != null)
            {
                barImages[i].gameObject.SetActive(false);
            }
        }
    }
    #endregion

    #region Coroutines
    private IEnumerator AmbientBarsLoop()
    {
        while (true)
        {
            float waitTime = Random.Range(randomIntervalMin, randomIntervalMax);
            yield return new WaitForSeconds(waitTime);

            if (flashRoutine == null)
            {
                int count = Random.Range(1, 4);
                ShowRandomBars(count, 4f, 26f, 0.14f, 0.42f);
                yield return new WaitForSeconds(randomBurstDuration);

                if (flashRoutine == null)
                {
                    HideAllBars();
                }
            }
        }
    }

    private IEnumerator SwitchFlashSequence()
    {
        float elapsed = 0f;

        while (elapsed < switchFlashDuration)
        {
            int count = Random.Range(5, barImages.Length + 1);
            ShowRandomBars(count, 8f, 65f, 0.35f, 0.85f);
            yield return new WaitForSeconds(switchStepInterval);
            elapsed += switchStepInterval;
        }

        HideAllBars();
        flashRoutine = null;
    }
    #endregion
}