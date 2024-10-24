using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler,IDragHandler,IEndDragHandler
{

public Transform parenToReturnTo = null;

public enum Food {Good, Bad}
public Food typeOfFood = Food.Good;

public void OnBeginDrag(PointerEventData eventData)
{
parenToReturnTo = this.transform.parent;
this.transform.SetParent (this.transform.parent.parent);

GetComponent<CanvasGroup>().blocksRaycasts = false;
}

public void OnDrag(PointerEventData eventData)
{
this.transform.position = eventData.position;
}

public void OnEndDrag(PointerEventData eventData)
{
this.transform.SetParent(parenToReturnTo);
GetComponent<CanvasGroup>().blocksRaycasts = true;
}
}
