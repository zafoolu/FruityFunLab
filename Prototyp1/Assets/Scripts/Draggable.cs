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
    public string foodType;
    public int points;

    // Statische Variablen, um die Gesamtwerte über alle Runden hinweg zu speichern
    public static int totalProtein = 0;
    public static int totalCarbs = 0;
    public static int totalEtc = 0;
    public static int totalCalories = 0;
    public static int totalVitamins = 0;
    public static int totalMinerals = 0;
    public static int totalPoints = 0;

    // Referenzen für die Sliders, die die Gesamtwerte anzeigen
    public Slider proteinSlider;
    public Slider carbsSlider;
    public Slider etcSlider;
    public Slider caloriesSlider;
    public Slider vitaminsSlider;
    public Slider mineralsSlider;

    // Die Anzahl an verfügbaren Discards und New Draws pro Runde
    public static int discardCharge = 1;
    public static int newDrawCharge = 1;

    public static int money = 0;

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

    // Methode, um die Karten abzuwerfen
    public void DiscardCards(DeckManager deckManager)
    {
        if (discardCharge <= 0)
        {
            Debug.LogWarning("Du hast keine Discard-Charge mehr für diese Runde!");
            return; // Keine Charges verfügbar
        }

        if (selectedCards.Count != 1)
        {
            Debug.LogWarning("Es muss genau eine Karte zum Abwerfen ausgewählt sein.");
            return; // Wenn mehr als 1 Karte ausgewählt ist, passiert nichts
        }

        // Nur eine Karte wird abgeworfen
        Draggable cardToDiscard = selectedCards[0];

        // Zerstöre die abgeworfene Karte
        Destroy(cardToDiscard.gameObject);

        // Leere die Liste der ausgewählten Karten
        selectedCards.Clear();
        selectedCardCount = 0;

        // Ziehe eine neue Karte
        deckManager.DrawCard(); // Eine Karte vom Deck wird gezogen

        // Verringere die Anzahl der Discards für die Runde
        discardCharge--;

        Debug.Log("Eine Karte wurde abgeworfen und eine neue Karte wurde gezogen.");
    }

    // Funktion zum Kaufen von Discard-Charges
public void BuyDiscardCharge()
{
    int cost = 2; // Kosten für eine Discard-Charge

    if (money >= cost) // Überprüfen, ob genügend Geld vorhanden ist
    {
        money -= cost; // Kosten abziehen
        discardCharge++; // Charge hinzufügen
        Debug.Log($"Eine neue Discard-Charge wurde gekauft! Verbleibendes Geld: {money}");
    }
    else
    {
        Debug.LogWarning($"Nicht genug Geld! Du benötigst {cost} Moneten, hast aber nur {money}.");
    }
}

    // Neue Funktion zum Ziehen von Karten (New Draw)
    public void NewDraw(DeckManager deckManager)
    {
        if (newDrawCharge <= 0)
        {
            Debug.LogWarning("Du hast keine New Draw-Charge mehr für diese Runde!");
            return; // Keine Charges verfügbar
        }

        // Überprüfe, ob maximal 2 Karten ausgewählt sind
        if (selectedCards.Count > 2)
        {
            Debug.LogWarning("Du kannst nur maximal 2 Karten auswählen!");
            return; // Wenn mehr als 2 Karten ausgewählt sind, passiert nichts
        }

        // Berechne die Anzahl der Karten, die entfernt werden sollen
        int cardsToRemove = deckManager.handPanel.childCount - selectedCards.Count;

        // Entferne alle nicht ausgewählten Karten
        foreach (Transform child in deckManager.handPanel)
        {
            if (!selectedCards.Contains(child.GetComponent<Draggable>()))
            {
                Destroy(child.gameObject); // Zerstöre nicht ausgewählte Karten
            }
        }

        // Ziehe so viele Karten wie entfernt wurden
        for (int i = 0; i < cardsToRemove; i++)
        {
            deckManager.DrawCard();
        }

        // Verringere die Anzahl der New Draws für die Runde
        newDrawCharge--;

        Debug.Log($"{cardsToRemove} Karte(n) entfernt und {cardsToRemove} neue Karte(n) gezogen.");
    }

    // Funktion zum Kaufen von New Draw-Charges
public void BuyNewDrawCharge()
{
    int cost = 3; // Kosten für eine New Draw-Charge

    if (money >= cost) // Überprüfen, ob genügend Geld vorhanden ist
    {
        money -= cost; // Kosten abziehen
        newDrawCharge++; // Charge hinzufügen
        Debug.Log($"Eine neue New Draw-Charge wurde gekauft! Verbleibendes Geld: {money}");
    }
    else
    {
        Debug.LogWarning($"Nicht genug Geld! Du benötigst {cost} Moneten, hast aber nur {money}.");
    }
}

    // Play Methode, die beim Spielen der Karten aufgerufen wird
public void Play()
{
    GameObject playedCardsPanel = GameObject.Find("Arena");
    foreach (var card in selectedCards)
{
    Debug.Log($"Karte: {card.name}, Punkte: {card.points}");
}

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

    // Lokale Variablen für Komboprüfungen
    int fruitCount = 0;
    int vegetableCount = 0;
    int oilFatCount = 0;
    int meatCount = 0;
    int grainCount = 0;
    int fishCount = 0;

    int roundPoints = 0; // Punkte für diese Runde
    float comboMultiplier = 1f; // Multiplikator, der durch Kombos beeinflusst wird

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
        totalCalories += card.Calories;
        totalMinerals += card.Minerals;
        totalVitamins += card.Vitamins;

        // Punkte dieser Karte zur Rundenpunktzahl hinzufügen
        roundPoints += card.points;

        // Debug-Log, um die Werte der Karte zu überprüfen
        Debug.Log($"Karte: {card.name} | Food Type: {card.foodType} | Points: {card.points}");

        // Zähle die Karten basierend auf ihrem Food-Typ
        switch (card.foodType.ToLower())
        {
            case "obst":
                fruitCount++;
                break;
            case "gemüse":
                vegetableCount++;
                break;
            case "öl/fett":
                oilFatCount++;
                break;
            case "fleisch":
                meatCount++;
                break;
            case "getreide":
                grainCount++;
                break;
            case "fisch":
                fishCount++;
                break;
        }
    }

    // Kombo-Berechnungen
    if (fruitCount >= 2)
    {
        comboMultiplier = Mathf.Max(comboMultiplier, 2f); // Verdopple die Punkte
        Debug.Log("Obstsalat-Kombo ausgelöst! Punkte verdoppelt.");
    }

    if (vegetableCount >= 2 && oilFatCount >= 1)
    {
        comboMultiplier = Mathf.Max(comboMultiplier, 3f); // Verdreifache die Punkte
        Debug.Log("Gemüsesalat-Kombo ausgelöst! Punkte verdreifacht.");
    }

    // Fleisch/Fisch + Beilage (Gemüse)
    if ((meatCount >= 1 || fishCount >= 1) && vegetableCount >= 1)
    {
        comboMultiplier = Mathf.Max(comboMultiplier, 2f); // Verdopple die Punkte
        Debug.Log("Fleisch mit Beilage-Kombo ausgelöst! Punkte verdoppelt.");
    }

    // Getreideprodukt + Fleisch/Fisch/Gemüse
    if (grainCount >= 1 && (meatCount >= 1 || fishCount >= 1 || vegetableCount >= 1))
    {
        comboMultiplier = Mathf.Max(comboMultiplier, 1.5f); // 1,5-fache Punkte
        Debug.Log("Nudeln mit Zutat-Kombo ausgelöst! Punkte um 1,5x erhöht.");
    }

    // Punkteberechnung mit Multiplikator
    roundPoints = Mathf.CeilToInt(roundPoints * comboMultiplier);

    // Addiere die Rundenpunkte zu den Gesamtpunkten
    totalPoints += roundPoints;

    // Ausgabe der aktuellen Gesamtwerte in der Konsole
    Debug.Log($"Punkte in dieser Runde: {roundPoints}");
    Debug.Log($"Gesamtpunkte: {totalPoints}");
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
    if (caloriesSlider != null) caloriesSlider.value = totalCalories;
    if (vitaminsSlider != null) vitaminsSlider.value = totalVitamins;
    if (mineralsSlider != null) mineralsSlider.value = totalMinerals;

    // Reset der Zähler
    selectedCardCount = 0;
    selectedCards.Clear();
}
}