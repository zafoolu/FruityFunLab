using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{

public Draggable.Food typeOfFood = Draggable.Food.Good;

public void OnPointerEnter(PointerEventData eventData)
{

}

public void OnPointerExit(PointerEventData eventData)
{
   
}


public void OnDrop (PointerEventData eventData)
{
    Debug.Log("OnDrop to " + gameObject.name);
Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
if (d != null && typeOfFood == d.typeOfFood)
{
    d.parenToReturnTo = this.transform;
}
}
}
