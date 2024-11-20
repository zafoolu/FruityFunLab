using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    // Maximal erlaubte Anzahl an Karten in dieser DropZone
    public int maxCards = 3;
    
    // Die Anzahl der Karten, die aktuell in der DropZone sind
    public int currentCards = 0;

    void Update ()
    {
        currentCards = this.transform.childCount;
    }

    public void OnPointerEnter(PointerEventData eventData) 
    { 
        // Optional: Hier könnte eine visuelle Anzeige hinzugefügt werden,
        // wenn der Mauszeiger in die DropZone eintritt.
    }

    public void OnPointerExit(PointerEventData eventData) 
    { 
        // Optional: Hier könnte eine visuelle Anzeige hinzugefügt werden,
        // wenn der Mauszeiger die DropZone verlässt.
    }

    public void OnDrop(PointerEventData eventData)
    {
        // Prüfen, ob die DropZone bereits die maximale Anzahl an Karten hat
        if (currentCards >= maxCards)
        {
            Debug.Log("Die DropZone hat bereits die maximale Anzahl an Karten.");
            return; // Verhindert das Hinzufügen der Karte, wenn die maximale Anzahl erreicht ist
        }

        Debug.Log("OnDrop to " + gameObject.name);
        
        // Holen der Draggable-Komponente von der gezogenen Karte
        Draggable draggable = eventData.pointerDrag.GetComponent<Draggable>();
        if (draggable != null)
        {
            // Karte in der DropZone platzieren
            draggable.parenToReturnTo = this.transform;

            // Karte als Kind in der DropZone platzieren
            draggable.transform.SetParent(this.transform);

            // Die aktuelle Anzahl an Karten in der DropZone erhöhen
        }
    }
}