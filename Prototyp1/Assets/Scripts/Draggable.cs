using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    public enum Food { Good, Bad }
    public Food typeOfFood;

    public bool selected;
    public bool isDragging;

    public Transform parenToReturnTo = null;
    private Vector3 originalScale;
    private Vector3 selectedScale;

    // Statische Variablen zur Verfolgung der Anzahl ausgewählter Karten und Liste der ausgewählten Karten
    private static int selectedCardCount = 0;
    private const int maxSelectedCards = 3;
    private static List<Draggable> selectedCards = new List<Draggable>();

    void Start()
    {
        // Speichern der Originalgröße und Berechnen der vergrößerten Skalierung
        originalScale = this.transform.localScale;
        selectedScale = originalScale * 1.2f;  // 20% größer
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        parenToReturnTo = this.transform.parent;
        this.transform.SetParent(this.transform.parent.parent); 
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Zurücksetzen des Elternobjekts auf das ursprüngliche
        this.transform.SetParent(parenToReturnTo);
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        this.transform.position = eventData.position;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Überprüfen, ob die Karte schon ausgewählt ist oder ob die maximale Auswahlanzahl erreicht ist
        if (!selected && selectedCardCount >= maxSelectedCards)
        {
            return; // Keine weitere Auswahl möglich
        }

        // Auswahlstatus umschalten
        selected = !selected;

        // Ausgewählte Kartenanzahl erhöhen oder verringern und zur Liste hinzufügen oder entfernen
        if (selected)
        {
            selectedCardCount++;
            selectedCards.Add(this);  // Zur Liste hinzufügen
        }
        else
        {
            selectedCardCount--;
            selectedCards.Remove(this);  // Aus Liste entfernen
        }

        UpdateScale();
    }

    // Methode zur Skalierungsanpassung basierend auf Auswahlstatus
    private void UpdateScale()
    {
        if (selected)
        {
            this.transform.localScale = selectedScale;  // Vergrößern
        }
        else
        {
            this.transform.localScale = originalScale;  // Zurück zur Originalgröße
        }
    }

    // Statische Methode zum Ausspielen der ausgewählten Karten
    public static void Play()
    {
        // Referenz auf das PlayedCardsPanel
        GameObject playedCardsPanel = GameObject.Find("Arena");

        // Alle bisherigen Karten im PlayedCardsPanel löschen
        foreach (Transform child in playedCardsPanel.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        // Überprüfen, ob es Karten gibt, die ausgespielt werden können
        if (selectedCards.Count == 0) return;

        // Jede ausgewählte Karte in das neue Parent-Objekt verschieben und als "gespielt" markieren
        foreach (Draggable card in selectedCards)
        {
            card.transform.SetParent(playedCardsPanel.transform); // Parent ändern
            card.selected = false; // Auswahlstatus zurücksetzen
            card.GetComponent<CanvasGroup>().blocksRaycasts = false; // Anklicken deaktivieren
            card.UpdateScale(); // Größe zurücksetzen
        }

        // Auswahlzähler zurücksetzen und Liste leeren
        selectedCardCount = 0;
        selectedCards.Clear();
    }
}