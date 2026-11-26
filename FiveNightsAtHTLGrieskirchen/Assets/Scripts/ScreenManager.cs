using System.Collections;
using UnityEngine;
using UnityEngine.UI;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public enum GameScreen
{
    Office,
    PC,
    Handy,
    Stundenplan
}

public class ScreenManager : MonoBehaviour
{
    #region References
    public GameObject pcPanel;
    public GameObject handyPanel;
    public GameObject stundenplanPanel;
    public CameraSystem cameraSystem;
    #endregion

    #region Monitor Flip Animation
    public Image monitorFlipImage;
    public Sprite[] monitorFlipFrames;
    public float flipFramesPerSecond = 30f;
    #endregion

    #region Monitor Bar Layout
    public RectTransform monitorBarRect;
    public float pcBarOffsetX = -260f;
    #endregion

    #region State
    public GameScreen currentScreen = GameScreen.Office;
    public bool isFlipping;
    public bool IsInOffice => currentScreen == GameScreen.Office && !isFlipping;

    private Vector2 officeBarPosition;
    private bool hasStoredBarPosition;
    #endregion

    #region Unity Lifecycle
    private void Awake()
    {
        if (monitorBarRect != null)
        {
            officeBarPosition = monitorBarRect.anchoredPosition;
            hasStoredBarPosition = true;
        }
    }

    private void Start()
    {
        if (monitorFlipImage != null)
        {
            monitorFlipImage.gameObject.SetActive(false);
        }

        SetScreenImmediate(GameScreen.Office);
    }

    private void Update()
    {
        bool escPressed = false;

#if ENABLE_INPUT_SYSTEM
        if (Keyboard.current != null)
        {
            escPressed = Keyboard.current.escapeKey.wasPressedThisFrame;
        }
#else
        escPressed = Input.GetKeyDown(KeyCode.Escape);
#endif

        if (escPressed && !isFlipping && currentScreen != GameScreen.Office)
        {
            if (currentScreen == GameScreen.PC)
            {
                StartCoroutine(ClosePCWithFlip());
            }
            else
            {
                SetScreenImmediate(GameScreen.Office);
            }
        }
    }
    #endregion

    #region Screen Actions
    public bool TogglePC()
    {
        if (isFlipping) return false;

        if (currentScreen == GameScreen.Office)
        {
            StartCoroutine(OpenPCWithFlip());
            return true;
        }

        if (currentScreen == GameScreen.PC)
        {
            StartCoroutine(ClosePCWithFlip());
            return true;
        }

        return false;
    }

    public void ReturnToOffice()
    {
        if (isFlipping) return;

        if (currentScreen == GameScreen.PC)
        {
            StartCoroutine(ClosePCWithFlip());
        }
        else
        {
            SetScreenImmediate(GameScreen.Office);
        }
    }

    private IEnumerator OpenPCWithFlip()
    {
        isFlipping = true;
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

        if (monitorFlipImage != null && monitorFlipFrames != null && monitorFlipFrames.Length > 0)
        {
            float frameDelay = 1f / Mathf.Max(1f, flipFramesPerSecond);
            monitorFlipImage.sprite = monitorFlipFrames[0];
            monitorFlipImage.gameObject.SetActive(true);

            for (int i = 0; i < monitorFlipFrames.Length; i++)
            {
                monitorFlipImage.sprite = monitorFlipFrames[i];
                yield return new WaitForSeconds(frameDelay);
            }

            monitorFlipImage.gameObject.SetActive(false);
        }

        SetScreenImmediate(GameScreen.PC);
        isFlipping = false;
    }

    private IEnumerator ClosePCWithFlip()
    {
        isFlipping = true;
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

        if (pcPanel != null)
        {
            pcPanel.SetActive(false);
        }

        if (monitorFlipImage != null && monitorFlipFrames != null && monitorFlipFrames.Length > 0)
        {
            float frameDelay = 1f / Mathf.Max(1f, flipFramesPerSecond);
            monitorFlipImage.sprite = monitorFlipFrames[monitorFlipFrames.Length - 1];
            monitorFlipImage.gameObject.SetActive(true);

            for (int i = monitorFlipFrames.Length - 1; i >= 0; i--)
            {
                monitorFlipImage.sprite = monitorFlipFrames[i];
                yield return new WaitForSeconds(frameDelay);
            }

            monitorFlipImage.gameObject.SetActive(false);
        }

        SetScreenImmediate(GameScreen.Office);
        isFlipping = false;
    }

    private void SetScreenImmediate(GameScreen targetScreen)
    {
        currentScreen = targetScreen;

        if (pcPanel != null)
        {
            pcPanel.SetActive(targetScreen == GameScreen.PC);
        }

        if (handyPanel != null)
        {
            handyPanel.SetActive(targetScreen == GameScreen.Handy);
        }

        if (stundenplanPanel != null)
        {
            stundenplanPanel.SetActive(targetScreen == GameScreen.Stundenplan);
        }

        UpdateMonitorBarLayout(targetScreen);

        if (targetScreen == GameScreen.PC && cameraSystem != null)
        {
            cameraSystem.OnMonitorOpened();
        }

        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    private void UpdateMonitorBarLayout(GameScreen targetScreen)
    {
        if (monitorBarRect == null || !hasStoredBarPosition) return;

        if (targetScreen == GameScreen.PC)
        {
            monitorBarRect.anchoredPosition = new Vector2(officeBarPosition.x + pcBarOffsetX, officeBarPosition.y);
        }
        else
        {
            monitorBarRect.anchoredPosition = officeBarPosition;
        }
    }
    #endregion
}