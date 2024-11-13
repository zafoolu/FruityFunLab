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

    private static int selectedCardCount = 0;
    private const int maxSelectedCards = 3;
    private static List<Draggable> selectedCards = new List<Draggable>();

    void Start()
    {
        originalScale = this.transform.localScale;
        selectedScale = originalScale * 1.2f;
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

    public static void Play()
    {
        GameObject playedCardsPanel = GameObject.Find("Arena");

        foreach (Transform child in playedCardsPanel.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        if (selectedCards.Count == 0) return;

        foreach (Draggable card in selectedCards)
        {
            card.transform.SetParent(playedCardsPanel.transform);
            card.selected = false;
            card.GetComponent<CanvasGroup>().blocksRaycasts = false;
            card.UpdateScale();
        }

        selectedCardCount = 0;
        selectedCards.Clear();
    }
}