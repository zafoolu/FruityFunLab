using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    // Maximale Anzahl von Karten, die in diese Zone gezogen werden können
    public int maxCardsInZone = 3;
    // Liste der Karten, die sich aktuell in dieser Zone befinden
    private List<Draggable> cardsInZone = new List<Draggable>();

    // Methode, die aufgerufen wird, wenn eine Karte über der Zone schwebt
    public void OnPointerEnter(PointerEventData eventData) { }

    // Methode, die aufgerufen wird, wenn eine Karte den Bereich verlässt
    public void OnPointerExit(PointerEventData eventData) { }

    // Methode, die aufgerufen wird, wenn eine Karte in die Zone gezogen wird
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop to " + gameObject.name);

        // Hole das Draggable-Skript der gezogenen Karte
        Draggable draggable = eventData.pointerDrag.GetComponent<Draggable>();

        // Wenn ein Draggable-Skript existiert
        if (draggable != null)
        {
            // Überprüfen, ob noch Platz für weitere Karten ist
            if (cardsInZone.Count < maxCardsInZone)
            {
                // Setze den Parent der Karte auf diese Zone
                draggable.parenToReturnTo = this.transform;

                // Füge die Karte zur Liste der Karten in der Zone hinzu
                cardsInZone.Add(draggable);
            }
            else
            {
                Debug.Log("Maximale Anzahl an Karten erreicht!");
            }
        }
    }
}