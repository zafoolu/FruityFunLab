using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public int maxCards = 3;
    public int currentCards = 0;

    void Update()
    {
        currentCards = this.transform.childCount;
    }

    public void OnPointerEnter(PointerEventData eventData) { }

    public void OnPointerExit(PointerEventData eventData) { }

    public void OnDrop(PointerEventData eventData)
    {
        if (currentCards >= maxCards)
        {
            Debug.Log("Die DropZone hat bereits die maximale Anzahl an Karten.");
            return;
        }

        Debug.Log("OnDrop to " + gameObject.name);

        Draggable draggable = eventData.pointerDrag.GetComponent<Draggable>();

        if (draggable != null)
        {
            draggable.parenToReturnTo = this.transform;
            draggable.transform.SetParent(this.transform);
        }
    }
}