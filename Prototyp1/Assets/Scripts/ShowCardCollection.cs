using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CardCollectionManager : MonoBehaviour
{
    public Transform cardCollectionPanel;
    public GameObject cardPrefab;
    public List<Card> initialCardCollection;

    void Start()
    {
        DisplayCollection(initialCardCollection);
    }

    public void DisplayCollection(List<Card> cards)
    {
        foreach (Transform child in cardCollectionPanel)
        {
            Destroy(child.gameObject);
        }

        foreach (Card card in cards)
        {
            GameObject cardInstance = Instantiate(cardPrefab, cardCollectionPanel);
            Image cardImage = cardInstance.GetComponent<Image>();

            if (cardImage != null)
            {
                cardImage.sprite = card.cardImage;
            }

            Draggable draggable = cardInstance.AddComponent<Draggable>();
            draggable.Protein = card.Protein;
            draggable.Carbs = card.Carbs;
            draggable.Etc = card.Etc;
            draggable.Calories = card.Calories;
            draggable.Vitamins = card.Vitamins;
            draggable.Minerals = card.Minerals;
            draggable.foodType = card.foodType;
            draggable.points = card.points;

            Debug.Log($"Karte hinzugefügt: {card.cardName}");

            Destroy(draggable);
        }
    }

    public void ToggleCollectionView()
    {
        bool isActive = cardCollectionPanel.gameObject.activeSelf;
        cardCollectionPanel.gameObject.SetActive(!isActive);
    }
}