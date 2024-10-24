using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler,IDragHandler,IEndDragHandler
{


public void OnBeginDrag(PointerEventData eventData)
{

}

public void OnDrag(PointerEventData eventData)
{
this.transform.position = eventData.position;
}

public void OnEndDrag(PointerEventData eventData)
{

}
}
