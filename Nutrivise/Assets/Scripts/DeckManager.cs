using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DeckManager : MonoBehaviour
{
    public List<Card> deck;
    public GameObject cardPrefab;
    public Transform handPanel;
    public GameObject deckImage;
    public static int cardsDrawn = 0; 
    public int maxCards = 9;  


    public int clickCount = 0;

    void Start()
    {
        ShuffleDeck();
    }

    public void HandleDeckClick()
    {
        if (clickCount >= maxCards)
        {
            Debug.Log("Du hast bereits 7 Karten gezogen. Weitere Ziehungen sind nicht mehr möglich.");
            return;
        }

        
        FindObjectOfType<AudioManager>().Play("draw_sound");
        DrawCard();
        clickCount++;

        if (clickCount >= maxCards)
        {
            Debug.Log("Maximale Kartenanzahl (7) erreicht. Weitere Ziehungen sind deaktiviert.");
        }
    }

    public void DrawCard()
    {
        if (deck.Count > 0)
        {
            Card drawnCard = deck[0];
            deck.RemoveAt(0);

            GameObject cardInstance = Instantiate(cardPrefab, handPanel);
            Image cardImage = cardInstance.GetComponent<Image>();
            if (cardImage != null)
            {
                cardImage.sprite = drawnCard.cardImage;
            }

            Draggable draggable = cardInstance.AddComponent<Draggable>();
            draggable.Protein = drawnCard.Protein;
            draggable.Carbs = drawnCard.Carbs;
            draggable.Etc = drawnCard.Etc;
            draggable.Calories = drawnCard.Calories;
            draggable.Vitamins = drawnCard.Vitamins;
            draggable.Minerals = drawnCard.Minerals;
            draggable.foodType = drawnCard.foodType;
            draggable.points = drawnCard.points;

            Debug.Log($"Gezogene Karte: {drawnCard.cardName} | Protein: {draggable.Protein}, Carbs: {draggable.Carbs}, Etc: {draggable.Etc}");

            if (deck.Count == 0)
            {
                deckImage.SetActive(false);
            }
        }
        else
        {
            Debug.Log("Deck ist leer!");
        }
    }

    private void ShuffleDeck()
    {
        System.Random rng = new System.Random();
        int n = deck.Count;

        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
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
    public float value;
    public Sprite cardImage;
    public float Protein;
    public float Carbs;
    public float Etc;
    public float Calories;
    public float Vitamins;
    public float Minerals;
    public string foodType;
    public float points;
}