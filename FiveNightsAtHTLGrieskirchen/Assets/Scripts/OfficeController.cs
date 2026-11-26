using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class OfficeController : MonoBehaviour
{
    #region References
    public ScreenManager screenManager;
    public Image officeImage;
    public TMP_Text lightHintText;
    public float lightHintDuration = 5f;
    public float lightHintFadeDuration = 0.5f;

    public Sprite doorOpenLightOff;
    public Sprite doorOpenLightOn;
    public Sprite doorClosedLightOff;
    public Sprite doorClosedLightOn;
    #endregion

    #region State
    public bool isDoorClosed;
    public bool isLightOn;
    #endregion

    #region Unity Lifecycle
    private void Start()
    {
        UpdateOfficeVisual();

        if (lightHintText != null)
        {
            StartCoroutine(ShowLightHint());
        }
    }

    private void Update()
    {
        bool canUseLight = screenManager == null || screenManager.IsInOffice;
        bool lightPressed = false;

        if (canUseLight)
        {
#if ENABLE_INPUT_SYSTEM
            if (Keyboard.current != null)
            {
                lightPressed = Keyboard.current.cKey.isPressed;
            }
#else
            lightPressed = Input.GetKey(KeyCode.C);
#endif
        }

        if (lightPressed != isLightOn)
        {
            isLightOn = lightPressed;
            UpdateOfficeVisual();
        }
    }
    #endregion

    #region Actions
    public void ToggleDoor()
    {
        if (screenManager != null && !screenManager.IsInOffice) return;

        isDoorClosed = !isDoorClosed;
        UpdateOfficeVisual();
    }

    private IEnumerator ShowLightHint()
    {
        lightHintText.gameObject.SetActive(true);
        Color c = lightHintText.color;
        c.a = 1f;
        lightHintText.color = c;

        yield return new WaitForSeconds(lightHintDuration);

        float elapsed = 0f;
        while (elapsed < lightHintFadeDuration)
        {
            elapsed += Time.deltaTime;
            c.a = 1f - Mathf.Clamp01(elapsed / lightHintFadeDuration);
            lightHintText.color = c;
            yield return null;
        }

        lightHintText.gameObject.SetActive(false);
    }

    private void UpdateOfficeVisual()
    {
        if (officeImage == null) return;

        if (isDoorClosed && isLightOn)
        {
            officeImage.sprite = doorClosedLightOn;
        }
        else if (isDoorClosed && !isLightOn)
        {
            officeImage.sprite = doorClosedLightOff;
        }
        else if (!isDoorClosed && isLightOn)
        {
            officeImage.sprite = doorOpenLightOn;
        }
        else
        {
            officeImage.sprite = doorOpenLightOff;
        }
    }
    #endregion
}