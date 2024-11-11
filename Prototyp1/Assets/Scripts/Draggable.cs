using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public enum Food { Good, Bad }
    public Food typeOfFood;

    public Transform parenToReturnTo = null;

    public void OnBeginDrag(PointerEventData eventData) 
    {
        parenToReturnTo = this.transform.parent;
        this.transform.SetParent(this.transform.parent.parent); 
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnEndDrag(PointerEventData eventData) //Setzt beim Ablegen den Panel zum Parent 
    {
        this.transform.SetParent(parenToReturnTo);
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        this.transform.position = eventData.position;
    }
}