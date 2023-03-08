using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WindowPanelDrag : MonoBehaviour , IDragHandler
{
    [SerializeField]
    RectTransform mainWindowRectTranform;
    [SerializeField]
    Canvas canvas;

    public void OnDrag(PointerEventData eventData)
    {
        mainWindowRectTranform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }
    
}
