
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public enum Food { Good, Bad }
    public Food typeOfFood;

    public bool isDragging;
    public Transform parenToReturnTo = null;

    private Vector3 originalScale;
    private Vector3 originalPosition;
    private bool isZoomed = false;
public EndscreenManager endscreenManager;
    private RectTransform rectTransform;
    private Canvas parentCanvas;
    private Vector2 dragStartPos;
    private const float dragThreshold = 10f;

    public Vector3 zoomedScale = new Vector3(2.5f, 2.5f, 2.5f);
    public Vector2 zoomedPosition = Vector2.zero;
public static int totalPlayedCards = 0;
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

    private static Draggable currentlyZoomedCard = null;

    public static int discardCharge = 1;
    public static int newDrawCharge = 1;

    public TextMeshProUGUI discardChargeText;
    public TextMeshProUGUI newDrawChargeText;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        parentCanvas = GetComponentInParent<Canvas>();
        originalScale = this.transform.localScale;
    }

    void Update()
    {
        if (discardChargeText != null)
        {
            discardChargeText.text = discardCharge.ToString();
        }

        if (newDrawChargeText != null)
        {
            newDrawChargeText.text = newDrawCharge.ToString();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        dragStartPos = eventData.position;
        isDragging = false;
    }

public void OnPointerUp(PointerEventData eventData)
{
    if (!isDragging)
    {
        // Überprüfe, ob die Karte sich im "Hand"-Panel befindet
        if (transform.parent != null && transform.parent.name == "Hand")
        {
            if (!isZoomed)
            {
                ZoomIn();
            }
            else
            {
                ZoomOut();
            }
        }
    }
}
   public void OnBeginDrag(PointerEventData eventData)
{
    // Wenn eine andere Karte gezoomt ist, entferne deren Zoom
    if (currentlyZoomedCard != null && currentlyZoomedCard != this)
    {
        currentlyZoomedCard.ZoomOut();
    }

    // Prüfen, ob diese Karte gezoomt ist, und den Zoom beenden
    if (isZoomed)
    {
        ZoomOut();
    }

    // Beginne den Drag-Vorgang
    float distance = Vector2.Distance(dragStartPos, eventData.position);
    if (distance > dragThreshold)
    {
        isDragging = true;
        FindObjectOfType<AudioManager>().Play("draw_sound");

        // Setze das Parent für das Ziehen (z. B. an das Canvas übergeben)
        parenToReturnTo = this.transform.parent;
        this.transform.SetParent(this.transform.parent.parent);

        // Blockiere Raycasts während des Ziehens
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
       FindObjectOfType<AudioManager>().Play("play_sound");
    }

    private void ZoomIn()
    {
        if (currentlyZoomedCard != null && currentlyZoomedCard != this)
        {
            currentlyZoomedCard.ZoomOut();
        }

        isZoomed = true;
        currentlyZoomedCard = this;

        originalPosition = rectTransform.anchoredPosition;

        if (parentCanvas == null)
        {
            return;
        }

        Camera canvasCamera = parentCanvas.worldCamera;
        if (canvasCamera == null)
        {
            canvasCamera = Camera.main;
        }

        if (canvasCamera == null)
        {
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
    // Bereinige das Spielfeld
    GameObject playedCardsPanel = GameObject.Find("Arena");

    foreach (Transform child in playedCardsPanel.transform)
    {
        Destroy(child.gameObject);
    }

    if (playedCardsPanel.transform.childCount == 0) return;

    int fruitCount = 0, vegetableCount = 0, oilFatCount = 0, meatCount = 0, grainCount = 0, fishCount = 0;
    int roundPoints = 0;
    float comboMultiplier = 1f;

    FindObjectOfType<AudioManager>().Play("play_sound");

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

    if (fruitCount >= 2) comboMultiplier = Mathf.Max(comboMultiplier, 2f);
    if (vegetableCount >= 2 && oilFatCount >= 1) comboMultiplier = Mathf.Max(comboMultiplier, 3f);
    if ((meatCount >= 1 || fishCount >= 1) && vegetableCount >= 1) comboMultiplier = Mathf.Max(comboMultiplier, 2f);
    if (grainCount >= 1 && (meatCount >= 1 || fishCount >= 1 || vegetableCount >= 1)) comboMultiplier = Mathf.Max(comboMultiplier, 1.5f);

    roundPoints = Mathf.CeilToInt(roundPoints * comboMultiplier);
    totalPoints += roundPoints;

    // Aktualisiere die Slider
    if (proteinSlider != null) proteinSlider.value = totalProtein;
    if (carbsSlider != null) carbsSlider.value = totalCarbs;
    if (etcSlider != null) etcSlider.value = totalEtc;
    if (caloriesSlider != null) caloriesSlider.value = totalCalories;
    if (vitaminsSlider != null) vitaminsSlider.value = totalVitamins;
    if (mineralsSlider != null) mineralsSlider.value = totalMinerals;

    // Am Ende des Plays: Überprüfe, ob das Spiel zu Ende ist
    if (endscreenManager != null)
    {
        endscreenManager.CheckGameOver();
    }
}

    public void NewDraw(DeckManager deckManager)
    {
        if (newDrawCharge > 0)
        {
            GameObject handPanel = GameObject.Find("Hand");

            if (handPanel == null)
            {
                return;
            }
            FindObjectOfType<AudioManager>().Play("discard_sound");
            int cardCount = handPanel.transform.childCount;

            foreach (Transform child in handPanel.transform)
            {
                Destroy(child.gameObject);
            }

            for (int i = 0; i < cardCount; i++)
            {
                if (deckManager != null)
                {
                    deckManager.DrawCard();
                }
            }
            newDrawCharge--;

            if (newDrawChargeText != null)
            {
                newDrawChargeText.text = newDrawCharge.ToString();
            }
        }
    }

public void Discard(DeckManager deckManager)
{
    if (discardCharge > 0) // Überprüfen, ob discardCharge größer als 0 ist
    {
        if (currentlyZoomedCard != null)
        {
            FindObjectOfType<AudioManager>().Play("discard_sound");
            Destroy(currentlyZoomedCard.gameObject);

            deckManager.DrawCard();

            discardCharge--;

            currentlyZoomedCard.ZoomOut();

            if (discardChargeText != null)
            {
                discardChargeText.text = discardCharge.ToString();
            }
        }
    }
    else
    {
        Debug.Log("Not enough discard charges to discard a card.");
    }
}

    public void BuyDiscard()
    {
        if (money >= 2)
        {
            money -= 2;
            discardCharge++;
            FindObjectOfType<AudioManager>().Play("coin_sound");

            if (discardChargeText != null)
            {
                discardChargeText.text = discardCharge.ToString();
            }
        }
    }

    public void BuyNewDraw()
    {
        if (money >= 3)
        {
            money -= 3;
            newDrawCharge++;
             FindObjectOfType<AudioManager>().Play("coin_sound");

            if (newDrawChargeText != null)
            {
                newDrawChargeText.text = newDrawCharge.ToString();
            }
        }
    }
    public static void ResetZoomedCard()
{
    if (currentlyZoomedCard != null)
    {
        currentlyZoomedCard.ZoomOut();
    }
}
}