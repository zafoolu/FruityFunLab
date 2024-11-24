using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;  // TextMeshPro verwenden

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public enum Food { Good, Bad }
    public Food typeOfFood;

    public bool isDragging;
    public Transform parenToReturnTo = null;

    private Vector3 originalScale;
    private Vector3 originalPosition;
    private bool isZoomed = false;

    private RectTransform rectTransform;
    private Canvas parentCanvas;
    private Vector2 dragStartPos;
    private const float dragThreshold = 10f;

    [Header("Zoom Settings")]
    public Vector3 zoomedScale = new Vector3(2.5f, 2.5f, 2.5f); // Skalierung beim Zoom
    public Vector2 zoomedPosition = Vector2.zero; // Zielposition beim Zoom (Canvas-Koordinaten)

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

    public static int money = 0;

    // Neue statische Referenz für die aktuell gezoomte Karte
    private static Draggable currentlyZoomedCard = null;

    // New Draw und Discard Charges
    public static int discardCharge = 1;
    public static int newDrawCharge = 1;

    // TextMeshPro-Referenzen für Anzeige der verbleibenden Charges
    public TextMeshProUGUI discardChargeText;
    public TextMeshProUGUI newDrawChargeText;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        parentCanvas = GetComponentInParent<Canvas>();
        originalScale = this.transform.localScale;
    }

    // Die Update-Methode wird jeden Frame aufgerufen
    void Update()
    {
        // Aktualisiere die Textfelder mit den aktuellen Werten der Charges (nur die Zahl, ohne Text davor)
        if (discardChargeText != null)
        {
            discardChargeText.text = discardCharge.ToString();  // Nur die Zahl anzeigen
        }

        if (newDrawChargeText != null)
        {
            newDrawChargeText.text = newDrawCharge.ToString();  // Nur die Zahl anzeigen
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        dragStartPos = eventData.position;
        isDragging = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isDragging) // Nur wenn kein Dragging aktiv ist
        {
            if (!isZoomed) // Kein Zoom im Discard-Modus
            {
                ZoomIn();
            }
            else
            {
                ZoomOut();
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        float distance = Vector2.Distance(dragStartPos, eventData.position);
        if (distance > dragThreshold)
        {
            isDragging = true;

            parenToReturnTo = this.transform.parent;
            this.transform.SetParent(this.transform.parent.parent);
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;
        this.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDragging) return;

        this.transform.SetParent(parenToReturnTo);
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    private void ZoomIn()
    {
        if (currentlyZoomedCard != null && currentlyZoomedCard != this)
        {
            currentlyZoomedCard.ZoomOut();
        }

        isZoomed = true;
        currentlyZoomedCard = this; // Diese Karte als aktuell gezoomt setzen

        originalPosition = rectTransform.anchoredPosition;

        if (parentCanvas == null)
        {
            Debug.LogError("Parent Canvas ist null. Zoom kann nicht ausgeführt werden.");
            return;
        }

        Camera canvasCamera = parentCanvas.worldCamera;
        if (canvasCamera == null)
        {
            canvasCamera = Camera.main;
        }

        if (canvasCamera == null)
        {
            Debug.LogError("Keine Kamera gefunden! Zoom kann nicht durchgeführt werden.");
            return;
        }

        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Vector2 canvasCenter = parentCanvas.GetComponent<RectTransform>().InverseTransformPoint(screenCenter);

        rectTransform.anchoredPosition = canvasCenter;
        this.transform.localScale = zoomedScale;
    }

    private void ZoomOut()
    {
        isZoomed = false;
        if (currentlyZoomedCard == this)
        {
            currentlyZoomedCard = null;
        }

        rectTransform.anchoredPosition = originalPosition;
        this.transform.localScale = originalScale;
    }

    public void Play()
    {
        GameObject playedCardsPanel = GameObject.Find("Arena");

        foreach (Transform child in playedCardsPanel.transform)
        {
            Destroy(child.gameObject);
        }

        if (playedCardsPanel.transform.childCount == 0) return;

        int fruitCount = 0, vegetableCount = 0, oilFatCount = 0, meatCount = 0, grainCount = 0, fishCount = 0;
        int roundPoints = 0;
        float comboMultiplier = 1f;

        foreach (Transform child in playedCardsPanel.transform)
        {
            Draggable card = child.GetComponent<Draggable>();

            if (card == null) continue;

            roundPoints += card.points;

            totalProtein += card.Protein;
            totalCarbs += card.Carbs;
            totalEtc += card.Etc;
            totalCalories += card.Calories;
            totalMinerals += card.Minerals;
            totalVitamins += card.Vitamins;

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

        // Komboslogik
        if (fruitCount >= 2) comboMultiplier = Mathf.Max(comboMultiplier, 2f);
        if (vegetableCount >= 2 && oilFatCount >= 1) comboMultiplier = Mathf.Max(comboMultiplier, 3f);
        if ((meatCount >= 1 || fishCount >= 1) && vegetableCount >= 1) comboMultiplier = Mathf.Max(comboMultiplier, 2f);
        if (grainCount >= 1 && (meatCount >= 1 || fishCount >= 1 || vegetableCount >= 1)) comboMultiplier = Mathf.Max(comboMultiplier, 1.5f);

        roundPoints = Mathf.CeilToInt(roundPoints * comboMultiplier);
        totalPoints += roundPoints;

        // Update der Slider-Werte
        if (proteinSlider != null) proteinSlider.value = totalProtein;
        if (carbsSlider != null) carbsSlider.value = totalCarbs;
        if (etcSlider != null) etcSlider.value = totalEtc;
        if (caloriesSlider != null) caloriesSlider.value = totalCalories;
        if (vitaminsSlider != null) vitaminsSlider.value = totalVitamins;
        if (mineralsSlider != null) mineralsSlider.value = totalMinerals;
    }

    // Methode für New Draw
    public void NewDraw(DeckManager deckManager)
    {
        if (newDrawCharge > 0)
        {
            GameObject playedCardsPanel = GameObject.Find("Arena");

            foreach (Transform child in playedCardsPanel.transform)
            {
                Destroy(child.gameObject);
            }

            deckManager.DrawCard(); // Angepasster Aufruf

            newDrawCharge--;  // New Draw Charge verringern
            Debug.Log("Neue Karte gezogen. Verbleibende Charges: " + newDrawCharge);
        }
        else
        {
            Debug.Log("Keine New Draw Charges verfügbar.");
        }
    }

    // Methode für Discard
    public void Discard(DeckManager deckManager)
    {
        GameObject playedCardsPanel = GameObject.Find("Arena");
        int cardCount = playedCardsPanel.transform.childCount;

        if (discardCharge > 0)
        {
            if (cardCount == 1)
            {
                Draggable cardToDiscard = playedCardsPanel.transform.GetChild(0).GetComponent<Draggable>();
                Destroy(cardToDiscard.gameObject);

                deckManager.DrawCard(); // Angepasster Aufruf

                discardCharge--;  // Discard Charge verringern
                Debug.Log("Karte verworfen. Verbleibende Charges: " + discardCharge);
            }
            else if (cardCount > 1)
            {
                Debug.Log("Es darf nur eine Karte in der Arena sein, um sie zu discarden.");
            }
            else
            {
                Debug.Log("Keine Karte in der Arena, die discarded werden kann.");
            }
        }
        else
        {
            Debug.Log("Keine Discard Charges verfügbar.");
        }
    }
}