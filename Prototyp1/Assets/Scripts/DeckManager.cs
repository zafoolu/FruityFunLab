using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public enum FoodType { Good, Bad }

//Klasse für Karte
[System.Serializable]
public class Card
{
    public string cardName;
    public int value;
    public Sprite cardImage;
    public FoodType foodType;
    public int Protein;
    public int Carbs;
    public int etc;
}

public class DeckManager : MonoBehaviour             // Deck
{
    public List<Card> deck;
    public GameObject cardPrefab;
    public Transform handPanel;
    public GameObject deckImage;

    public void DrawCard() //Draw Funktion
    {
        if (deck.Count > 0)
        {
            Card drawnCard = deck[0];
            Debug.Log(drawnCard.Protein);
            deck.RemoveAt(0);

            GameObject cardInstance = Instantiate(cardPrefab, handPanel);
            Image cardImage = cardInstance.GetComponent<Image>();
            if (cardImage != null)
            {
                cardImage.sprite = drawnCard.cardImage;
            }

            Draggable draggable = cardInstance.AddComponent<Draggable>();
            draggable.typeOfFood = (Draggable.Food)drawnCard.foodType;

            if (deck.Count == 0)
            {
                deckImage.SetActive(false);
            }
        }
        else
        {
            
        }
    }
}