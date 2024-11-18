using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DeckManager : MonoBehaviour
{
    public List<Card> deck;
    public GameObject cardPrefab;
    public Transform handPanel;
    public GameObject deckImage;

    // Zähler für die Klicks
    private int clickCount = 0;

    void Start()
    {
        // Mische das Deck zu Beginn
        ShuffleDeck();
    }

    // Funktion, die aufgerufen wird, wenn auf das Deck geklickt wird
    public void HandleDeckClick()
    {
        // Wenn bereits 7 Karten gezogen wurden, breche ab
        if (clickCount >= 7)
        {
            Debug.Log("Du hast bereits 7 Karten gezogen. Weitere Ziehungen sind nicht mehr möglich.");
            return;
        }

        // Ziehe eine Karte
        DrawCard();

        // Erhöhe den Klickzähler
        clickCount++;

        // Überprüfe, ob die maximale Anzahl der Ziehungen erreicht wurde
        if (clickCount >= 7)
        {
            Debug.Log("Maximale Kartenanzahl (7) erreicht. Weitere Ziehungen sind deaktiviert.");
        }
    }

    // Funktion zum Ziehen einer Karte
    public void DrawCard()
    {
        if (deck.Count > 0)
        {
            // Ziehe die oberste Karte aus dem Deck
            Card drawnCard = deck[0];
            deck.RemoveAt(0);

            // Erstelle eine Instanz der Karte im Handbereich
            GameObject cardInstance = Instantiate(cardPrefab, handPanel);
            Image cardImage = cardInstance.GetComponent<Image>();
            if (cardImage != null)
            {
                cardImage.sprite = drawnCard.cardImage;
            }

            // Füge das Draggable-Skript hinzu und setze die Werte
            Draggable draggable = cardInstance.AddComponent<Draggable>();
            draggable.Protein = drawnCard.Protein;
            draggable.Carbs = drawnCard.Carbs;
            draggable.Etc = drawnCard.Etc;
            draggable.Calories = drawnCard.Calories;
            draggable.Vitamins = drawnCard.Vitamins;
            draggable.Minerals = drawnCard.Minerals;
            draggable.foodType = drawnCard.foodType;
            draggable.points = drawnCard.points;

            // Debug-Log, um die Werte der gezogenen Karte zu überprüfen
            Debug.Log($"Gezogene Karte: {drawnCard.cardName} | Protein: {draggable.Protein}, Carbs: {draggable.Carbs}, Etc: {draggable.Etc}");

            // Deaktiviere das Deckbild, wenn keine Karten mehr vorhanden sind
            if (deck.Count == 0)
            {
                deckImage.SetActive(false);
            }
        }
        else
        {
            // Falls das Deck leer ist
            Debug.Log("Deck ist leer!");
        }
    }

    // Methode zum Mischen des Decks
    private void ShuffleDeck()
    {
        System.Random rng = new System.Random(); // Zufallszahlengenerator
        int n = deck.Count;
        
        // Fisher-Yates Shuffle-Algorithmus
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1); // Wählt einen Index zwischen 0 und n (einschließlich)
            Card value = deck[k];
            deck[k] = deck[n];
            deck[n] = value;
        }

        Debug.Log("Deck gemischt!");
    }
}

[System.Serializable]
public class Card
{
    public string cardName;
    public int value;
    public Sprite cardImage;
    public int Protein;
    public int Carbs;
    public int Etc;
    public int Calories;
    public int Vitamins;
    public int Minerals;
    public string foodType;
    public int points;
}