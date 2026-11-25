using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverCursor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Texture2D hoverCursor;
    public Vector2 cursorHotspot = Vector2.zero;

    private bool isHovering;

#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
    private const int IDC_HAND = 32649;

    [DllImport("user32.dll")]
    private static extern IntPtr LoadCursor(IntPtr hInstance, IntPtr lpCursorName);

    [DllImport("user32.dll")]
    private static extern IntPtr SetCursor(IntPtr hCursor);

    private IntPtr windowsHandCursor;
#endif

    private void Awake()
    {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        windowsHandCursor = LoadCursor(IntPtr.Zero, new IntPtr(IDC_HAND));
#endif
    }

    private void LateUpdate()
    {
        if (!isHovering || hoverCursor != null) return;

#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        if (windowsHandCursor != IntPtr.Zero)
        {
            SetCursor(windowsHandCursor);
        }
#endif
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;

        if (hoverCursor != null)
        {
            Cursor.SetCursor(hoverCursor, cursorHotspot, CursorMode.Auto);
        }
        else
        {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
            if (windowsHandCursor != IntPtr.Zero)
            {
                SetCursor(windowsHandCursor);
            }
#endif
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    private void OnDisable()
    {
        isHovering = false;
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}