using UnityEngine;
using UnityEngine.EventSystems;

public class FloorHoverBar : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    #region References
    public GameObject minimapEG;
    public GameObject minimapOG;
    public RectTransform camNameTextRect;
    #endregion

    #region Settings
    public float cooldown = 0.25f;
    public float ogTextOffsetY = -180f;
    #endregion

    #region State
    public bool isOGActive;
    private bool canTrigger = true;
    private float lastTriggerTime = -10f;
    private Vector2 egTextPosition;
    private bool hasStoredTextPosition;
    #endregion

    #region Unity Lifecycle
    private void Awake()
    {
        EnsureStoredTextPosition();
    }

    private void Start()
    {
        ApplyFloor();
    }

    private void OnEnable()
    {
        canTrigger = true;
    }
    #endregion

    #region Pointer Events
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!canTrigger) return;
        if (Time.unscaledTime - lastTriggerTime < cooldown) return;

        canTrigger = false;
        lastTriggerTime = Time.unscaledTime;
        ToggleFloor();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        canTrigger = true;
    }
    #endregion

    #region Actions
    public void ToggleFloor()
    {
        isOGActive = !isOGActive;
        ApplyFloor();
    }

    public void SetFloor(bool showOG)
    {
        isOGActive = showOG;
        ApplyFloor();
    }

    private void EnsureStoredTextPosition()
    {
        if (!hasStoredTextPosition && camNameTextRect != null)
        {
            egTextPosition = camNameTextRect.anchoredPosition;
            hasStoredTextPosition = true;
        }
    }

    private void ApplyFloor()
    {
        if (minimapEG != null)
        {
            minimapEG.SetActive(!isOGActive);
        }

        if (minimapOG != null)
        {
            minimapOG.SetActive(isOGActive);
        }

        EnsureStoredTextPosition();

        if (camNameTextRect != null && hasStoredTextPosition)
        {
            float targetY = isOGActive ? egTextPosition.y + ogTextOffsetY : egTextPosition.y;
            camNameTextRect.anchoredPosition = new Vector2(egTextPosition.x, targetY);
        }
    }
    #endregion
}