using UnityEngine;
using UnityEngine.UI;

public class UIFrameAnimator : MonoBehaviour
{
    public Sprite[] frames;
    public float framesPerSecond = 15f;

    private Image targetImage;
    private int currentFrame;
    private float timer;

    private void Awake()
    {
        targetImage = GetComponent<Image>();
    }

    private void OnEnable()
    {
        if (targetImage == null)
        {
            targetImage = GetComponent<Image>();
        }

        currentFrame = 0;
        timer = 0f;

        if (frames != null && frames.Length > 0 && targetImage != null)
        {
            targetImage.sprite = frames[0];
        }
    }

    private void Update()
    {
        if (frames == null || frames.Length == 0 || targetImage == null)
        {
            return;
        }

        timer += Time.deltaTime;
        if (timer >= 1f / framesPerSecond)
        {
            timer -= 1f / framesPerSecond;
            currentFrame = (currentFrame + 1) % frames.Length;
            targetImage.sprite = frames[currentFrame];
        }
    }
}