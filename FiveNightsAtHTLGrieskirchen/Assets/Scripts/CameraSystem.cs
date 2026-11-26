using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[Serializable]
public class CameraEntry
{
    public string camName;
    public Sprite camSprite;
    public Button camButton;
    public Image labelImage;
    public Sprite inactiveLabelSprite;
    public Sprite activeLabelSprite;
}

public class CameraSystem : MonoBehaviour
{
    #region References
    public Image camFeedImage;
    public TMP_Text camNameText;
    public CamStaticBars staticBars;
    public CameraEntry[] cameras;
    #endregion

    #region Button Visuals
    public Sprite inactiveButtonSprite;
    public Sprite activeButtonSprite;
    public Color inactiveButtonColor = new Color(0.75f, 0.75f, 0.75f, 1f);
    public Color activeButtonColor = new Color(0.45f, 1f, 0.45f, 1f);
    #endregion

    #region State
    public int currentCamIndex;
    #endregion

    #region Unity Lifecycle
    private void Awake()
    {
        if (cameras == null) return;

        for (int i = 0; i < cameras.Length; i++)
        {
            int index = i;
            if (cameras[i].camButton != null)
            {
                cameras[i].camButton.onClick.AddListener(() => SelectCamera(index));
            }
        }
    }
    #endregion

    #region Actions
    public void OnMonitorOpened()
    {
        if (cameras == null || cameras.Length == 0) return;

        currentCamIndex = Mathf.Clamp(currentCamIndex, 0, cameras.Length - 1);
        ApplyCameraVisuals();

        if (staticBars != null)
        {
            staticBars.TriggerSwitchFlash();
        }
    }

    public void SelectCamera(int index)
    {
        if (cameras == null || index < 0 || index >= cameras.Length) return;
        if (index == currentCamIndex) return;

        currentCamIndex = index;
        ApplyCameraVisuals();

        if (staticBars != null)
        {
            staticBars.TriggerSwitchFlash();
        }
    }

    private void ApplyCameraVisuals()
    {
        CameraEntry activeCam = cameras[currentCamIndex];

        if (camFeedImage != null && activeCam.camSprite != null)
        {
            camFeedImage.sprite = activeCam.camSprite;
        }

        if (camNameText != null)
        {
            camNameText.text = activeCam.camName;
        }

        for (int i = 0; i < cameras.Length; i++)
        {
            if (cameras[i].camButton == null) continue;

            bool isSelected = i == currentCamIndex;
            Image btnImg = cameras[i].camButton.GetComponent<Image>();

            if (btnImg != null)
            {
                if (activeButtonSprite != null && inactiveButtonSprite != null)
                {
                    btnImg.sprite = isSelected ? activeButtonSprite : inactiveButtonSprite;
                    btnImg.color = Color.white;
                }
                else
                {
                    btnImg.color = isSelected ? activeButtonColor : inactiveButtonColor;
                }
            }

            Image targetLabel = cameras[i].labelImage;
            if (targetLabel == null)
            {
                Image[] childImages = cameras[i].camButton.GetComponentsInChildren<Image>(true);
                for (int j = 0; j < childImages.Length; j++)
                {
                    if (childImages[j] != btnImg)
                    {
                        targetLabel = childImages[j];
                        break;
                    }
                }
            }

            if (targetLabel != null && cameras[i].activeLabelSprite != null && cameras[i].inactiveLabelSprite != null)
            {
                targetLabel.sprite = isSelected ? cameras[i].activeLabelSprite : cameras[i].inactiveLabelSprite;
                targetLabel.color = Color.white;
            }
        }
    }
    #endregion
}