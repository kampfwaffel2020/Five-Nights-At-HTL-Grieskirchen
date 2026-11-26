using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MonitorHoverBar : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    #region References
    public ScreenManager screenManager;
    public float cooldown = 0.25f;
    #endregion

    #region State
    private bool canTrigger = true;
    private float lastTriggerTime = -10f;
    #endregion

    #region Pointer Events
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!canTrigger || screenManager == null) return;
        if (Time.unscaledTime - lastTriggerTime < cooldown) return;

        if (screenManager.TogglePC())
        {
            canTrigger = false;
            lastTriggerTime = Time.unscaledTime;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        canTrigger = true;
    }
    #endregion
}