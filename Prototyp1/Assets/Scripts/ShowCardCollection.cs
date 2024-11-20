using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CardCollectionManager : MonoBehaviour
{
    public Transform cardCollectionPanel;  // Referenz zum UI-Panel, das die Karten enthält
    public GameObject cardPrefab;          // Das Prefab für eine Karte
    public List<Card> initialCardCollection; // Liste mit Karten, die angezeigt werden sollen

    void Start()
    {
        DisplayCollection(initialCardCollection);  // Karten beim Start anzeigen
    }

    public void DisplayCollection(List<Card> cards)
    {
        // Lösche vorhandene Karten im Panel, um Duplikate zu vermeiden
        foreach (Transform child in cardCollectionPanel)
        {
            Destroy(child.gameObject);
        }

        // Iteriere durch die Kartenliste und füge jede Karte zur Sammlung hinzu
        foreach (Card card in cards)
        {
            GameObject cardInstance = Instantiate(cardPrefab, cardCollectionPanel);  // Karte instanziieren
            Image cardImage = cardInstance.GetComponent<Image>();  // Bild der Karte zuweisen
            if (cardImage != null)
            {
                cardImage.sprite = card.cardImage;
            }

            // Füge das Draggable-Skript hinzu und weise die Werte zu
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

            // Entferne das Draggable-Skript nach der Zuweisung der Werte
            Destroy(draggable); // Zerstört das Draggable-Skript
        }
    }

    // Funktion, um das Panel ein-/auszublenden
    public void ToggleCollectionView()
    {
        bool isActive = cardCollectionPanel.gameObject.activeSelf;
        cardCollectionPanel.gameObject.SetActive(!isActive);
    }
}