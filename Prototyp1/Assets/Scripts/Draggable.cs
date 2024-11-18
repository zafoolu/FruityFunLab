using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Für Slider
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    public enum Food { Good, Bad } // Wird momentan nicht genutzt, aber bleibt vorhanden
    public Food typeOfFood;

    public bool selected;
    public bool isDragging;

    public Transform parenToReturnTo = null;
    private Vector3 originalScale;
    private Vector3 selectedScale;

    private static int selectedCardCount = 0;
    private const int maxSelectedCards = 3;
    private static List<Draggable> selectedCards = new List<Draggable>();

    // Hier werden die Werte für Protein, Carbs und Etc direkt im Draggable gespeichert
    public int Protein;
    public int Carbs;
    public int Etc;
    public int Calories;
    public int Vitamins;
    public int Minerals;

    // Statische Variablen, um die Gesamtwerte über alle Runden hinweg zu speichern
    public static int totalProtein = 0;
    public static int totalCarbs = 0;
    public static int totalEtc = 0;
    public static int totalCalories =0;
    public static int totalVitamins = 0;
    public static int totalMinerals= 0;

    // Referenzen für die Sliders, die die Gesamtwerte anzeigen
    public Slider proteinSlider;
    public Slider carbsSlider;
    public Slider etcSlider;
    public Slider caloriesSlider;
    public Slider vitaminsSlider;
    public Slider mineralsSlider;

    void Start()
    {
        originalScale = this.transform.localScale;
        selectedScale = originalScale * 1.2f;

        // Stelle sicher, dass die Sliders zu Beginn den aktuellen Gesamtwert anzeigen
        if (proteinSlider != null) proteinSlider.value = totalProtein;
        if (carbsSlider != null) carbsSlider.value = totalCarbs;
        if (etcSlider != null) etcSlider.value = totalEtc;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        parenToReturnTo = this.transform.parent;
        this.transform.SetParent(this.transform.parent.parent);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        this.transform.SetParent(parenToReturnTo);
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        this.transform.position = eventData.position;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!selected && selectedCardCount >= maxSelectedCards)
        {
            return;
        }

        selected = !selected;

        if (selected)
        {
            selectedCardCount++;
            selectedCards.Add(this);
        }
        else
        {
            selectedCardCount--;
            selectedCards.Remove(this);
        }

        UpdateScale();
    }

    private void UpdateScale()
    {
        if (selected)
        {
            this.transform.localScale = selectedScale;
        }
        else
        {
            this.transform.localScale = originalScale;
        }
    }

    // Play Methode, die beim Spielen der Karten aufgerufen wird
    public void Play()
    {
        GameObject playedCardsPanel = GameObject.Find("Arena");

        // Löschen der Karten in der Arena
        foreach (Transform child in playedCardsPanel.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        if (selectedCards.Count == 0)
        {
            Debug.LogWarning("Keine Karten ausgewählt.");
            return;
        }

        // Iteriere über alle ausgewählten Karten und addiere die Werte
        foreach (Draggable card in selectedCards)
        {
            card.transform.SetParent(playedCardsPanel.transform);
            card.selected = false;
            card.GetComponent<CanvasGroup>().blocksRaycasts = false;
            card.UpdateScale();

            // Werte der Karte zu den Gesamtwerten addieren
            totalProtein += card.Protein;
            totalCarbs += card.Carbs;
            totalEtc += card.Etc;
            totalCalories +=card.Calories;
            totalMinerals += card.Minerals;
            totalVitamins += card.Vitamins;

            // Debug-Log, um die Werte der Karte zu überprüfen
            Debug.Log($"Karte: {card.name} | Protein: {card.Protein}, Carbs: {card.Carbs}, Etc: {card.Etc}");
        }

        // Ausgabe der aktuellen Gesamtwerte in der Konsole
        Debug.Log($"Total Protein (gesamt): {totalProtein}");
        Debug.Log($"Total Carbs (gesamt): {totalCarbs}");
        Debug.Log($"Total Etc (gesamt): {totalEtc}");
        Debug.Log($"Total ca (gesamt): {totalCalories}");
        Debug.Log($"Total v (gesamt): {totalVitamins}");
        Debug.Log($"Total m (gesamt): {totalMinerals}");


        // Update die Slider, damit sie die neuesten Werte anzeigen
        if (proteinSlider != null) proteinSlider.value = totalProtein;
        if (carbsSlider != null) carbsSlider.value = totalCarbs;
        if (etcSlider != null) etcSlider.value = totalEtc;
        if(caloriesSlider != null) caloriesSlider.value = totalCalories;
        if(vitaminsSlider != null)vitaminsSlider.value = totalVitamins;
        if(mineralsSlider !=null)mineralsSlider.value = totalMinerals;

        // Reset der Zähler
        selectedCardCount = 0;
        selectedCards.Clear();
    }
}