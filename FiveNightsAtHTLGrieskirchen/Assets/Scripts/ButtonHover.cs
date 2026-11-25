using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text buttonText;
    public string hoverPrefix = ">> ";

    private string originalText;

    private void Awake()
    {
        if (buttonText == null)
        {
            buttonText = GetComponentInChildren<TMP_Text>();
        }

        if (buttonText != null)
        {
            originalText = buttonText.text;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (buttonText != null)
        {
            buttonText.text = hoverPrefix + originalText;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (buttonText != null)
        {
            buttonText.text = originalText;
        }
    }

    private void OnDisable()
    {
        if (buttonText != null)
        {
            buttonText.text = originalText;
        }
    }
}