using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragFrame : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    Vector2 mousePos;
    RectTransform rect;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        mousePos = Input.mousePosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 offset = new Vector2(Input.mousePosition.x - mousePos.x, Input.mousePosition.y - mousePos.y);
        mousePos = Input.mousePosition;
        rect.anchoredPosition += offset;
    }
}
