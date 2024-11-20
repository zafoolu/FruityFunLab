using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    private static int selectedCardCount = 0;
    private const int maxSelectedCards = 3;
    private static List<Draggable> selectedCards = new List<Draggable>();

    public int Protein;
    public int Carbs;
    public int Etc;
    public int Calories;
    public int Vitamins;
    public int Minerals;
    public string foodType;
    public int points;

    public static int totalProtein = 0;
    public static int totalCarbs = 0;
    public static int totalEtc = 0;
    public static int totalCalories = 0;
    public static int totalVitamins = 0;
    public static int totalMinerals = 0;
    public static int totalPoints = 0;

    public Slider proteinSlider;
    public Slider carbsSlider;
    public Slider etcSlider;
    public Slider caloriesSlider;
    public Slider vitaminsSlider;
    public Slider mineralsSlider;

    public static int discardCharge = 1;
    public static int newDrawCharge = 1;
    public static int money = 0;

    void Start()
    {
        originalScale = this.transform.localScale;
        selectedScale = originalScale * 1.2f;

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
        this.transform.localScale = selected ? selectedScale : originalScale;
    }

    public void DiscardCards(DeckManager deckManager)
    {
        if (discardCharge <= 0 || selectedCards.Count != 1) return;

        Draggable cardToDiscard = selectedCards[0];
        Destroy(cardToDiscard.gameObject);

        selectedCards.Clear();
        selectedCardCount = 0;

        deckManager.DrawCard();
        discardCharge--;
    }

    public void BuyDiscardCharge()
    {
        int cost = 2;

        if (money >= cost)
        {
            money -= cost;
            discardCharge++;
        }
    }

    public void NewDraw(DeckManager deckManager)
    {
        if (newDrawCharge <= 0 || selectedCards.Count > 2) return;

        int cardsToRemove = deckManager.handPanel.childCount - selectedCards.Count;

        foreach (Transform child in deckManager.handPanel)
        {
            if (!selectedCards.Contains(child.GetComponent<Draggable>()))
            {
                Destroy(child.gameObject);
            }
        }

        for (int i = 0; i < cardsToRemove; i++)
        {
            deckManager.DrawCard();
        }

        newDrawCharge--;
    }

    public void BuyNewDrawCharge()
    {
        int cost = 3;

        if (money >= cost)
        {
            money -= cost;
            newDrawCharge++;
        }
    }

    public void Play()
    {
        GameObject playedCardsPanel = GameObject.Find("Arena");

        foreach (Transform child in playedCardsPanel.transform)
        {
            Destroy(child.gameObject);
        }

        if (selectedCards.Count == 0) return;

        int fruitCount = 0, vegetableCount = 0, oilFatCount = 0, meatCount = 0, grainCount = 0, fishCount = 0;
        int roundPoints = 0;
        float comboMultiplier = 1f;

        foreach (Draggable card in selectedCards)
        {
            card.transform.SetParent(playedCardsPanel.transform);
            card.selected = false;
            card.GetComponent<CanvasGroup>().blocksRaycasts = false;
            card.UpdateScale();

            totalProtein += card.Protein;
            totalCarbs += card.Carbs;
            totalEtc += card.Etc;
            totalCalories += card.Calories;
            totalMinerals += card.Minerals;
            totalVitamins += card.Vitamins;

            roundPoints += card.points;

            switch (card.foodType.ToLower())
            {
                case "obst": fruitCount++; break;
                case "gemüse": vegetableCount++; break;
                case "öl/fett": oilFatCount++; break;
                case "fleisch": meatCount++; break;
                case "getreide": grainCount++; break;
                case "fisch": fishCount++; break;
            }
        }

        if (fruitCount >= 2) comboMultiplier = Mathf.Max(comboMultiplier, 2f);
        if (vegetableCount >= 2 && oilFatCount >= 1) comboMultiplier = Mathf.Max(comboMultiplier, 3f);
        if ((meatCount >= 1 || fishCount >= 1) && vegetableCount >= 1) comboMultiplier = Mathf.Max(comboMultiplier, 2f);
        if (grainCount >= 1 && (meatCount >= 1 || fishCount >= 1 || vegetableCount >= 1)) comboMultiplier = Mathf.Max(comboMultiplier, 1.5f);

        roundPoints = Mathf.CeilToInt(roundPoints * comboMultiplier);
        totalPoints += roundPoints;

        if (proteinSlider != null) proteinSlider.value = totalProtein;
        if (carbsSlider != null) carbsSlider.value = totalCarbs;
        if (etcSlider != null) etcSlider.value = totalEtc;
        if (caloriesSlider != null) caloriesSlider.value = totalCalories;
        if (vitaminsSlider != null) vitaminsSlider.value = totalVitamins;
        if (mineralsSlider != null) mineralsSlider.value = totalMinerals;

        selectedCardCount = 0;
        selectedCards.Clear();
    }
}