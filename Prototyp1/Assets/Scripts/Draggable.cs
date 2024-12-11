
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler    
{

public Animator obstsalatAnim;

public GameObject currentReaction;
public GameObject sadReaction;
public GameObject happyReaction;
public GameObject neutralReaction;

    public enum Food { Good, Bad }
    public Food typeOfFood;
private bool wasZoomedBeforeInput = false;
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
    public float Protein;
    public float Carbs;
    public float Etc;
    public float Calories;
    public float Vitamins;
    public float Minerals;
    public string foodType;
    public float points;

    public static float totalProtein = 0;
    public static float totalCarbs = 0;
    public static float totalEtc = 0;
    public static float totalCalories = 0;
    public static float totalVitamins = 0;
    public static float totalMinerals = 0;
    public static float totalPoints = 0;

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
        currentReaction = neutralReaction;
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

    
    wasZoomedBeforeInput = isZoomed;
}

public void OnPointerUp(PointerEventData eventData)
{
    if (!isDragging)
    {
        
        if (transform.parent != null && transform.parent.name == "Hand")
        {
        
            if (!isZoomed && !wasZoomedBeforeInput)
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
    
    if (currentlyZoomedCard != null && currentlyZoomedCard != this)
    {
        currentlyZoomedCard.ZoomOut();
    }

    
    if (isZoomed)
    {
        ZoomOut();
    }

    
    float distance = Vector2.Distance(dragStartPos, eventData.position);
    if (distance > dragThreshold)
    {
        isDragging = true;
        FindObjectOfType<AudioManager>().Play("draw_sound");

        
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
    PersonaManager personaManager = FindObjectOfType<PersonaManager>();

    if (personaManager != null && personaManager.currentPersona != null)
    {
        // Die Reaktion standardmäßig auf neutral setzen
        neutralReaction.SetActive(true);
        happyReaction.SetActive(false);
        sadReaction.SetActive(false);
    }
    else
    {
        Debug.LogWarning("Keine Persona oder PersonaManager gefunden!");
    }

    GameObject playedCardsPanel = GameObject.Find("Arena");

    foreach (Transform child in playedCardsPanel.transform)
    {
        Destroy(child.gameObject);
    }

    if (playedCardsPanel.transform.childCount == 0) return;

    int fruitCount = 0, vegetableCount = 0, oilFatCount = 0, meatCount = 0, grainCount = 0, fishCount = 0, dairyCount = 0;
    float roundPoints = 0;
    float comboMultiplier = 1f;
    bool containsDislikedFood = false;

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

        if (personaManager.currentPersona.Dislikes.Contains(card.foodType.ToLower()))
        {
            containsDislikedFood = true;
            sadReaction.SetActive(true);
            neutralReaction.SetActive(false);
            happyReaction.SetActive(false);
            FindObjectOfType<AudioManager>().Play("angry_sound");
        }

        switch (card.foodType.ToLower())
        {
            case "obst": fruitCount++; break;
            case "gemüse": vegetableCount++; break;
            case "öl/fett": oilFatCount++; break;
            case "fleisch": meatCount++; break;
            case "getreide": grainCount++; break;
            case "fisch": fishCount++; break;
            case "milchprodukt": dairyCount++; break;
        }
    }

    if (containsDislikedFood)
    {
        comboMultiplier = 1f;
    }
    else
    {
        if (fruitCount >= 2)
        {
            AnimationManager animationManager = FindObjectOfType<AnimationManager>();
            animationManager.TriggerObstSalatAnimation();
            comboMultiplier = Mathf.Max(comboMultiplier, 2f);
        }

        if (vegetableCount >= 2 && oilFatCount >= 1) comboMultiplier = Mathf.Max(comboMultiplier, 3f);
        if (meatCount >= 1 && vegetableCount >= 1) comboMultiplier = Mathf.Max(comboMultiplier, 2f);
        if (fishCount >= 1 && vegetableCount >= 1) comboMultiplier = Mathf.Max(comboMultiplier, 2f);
        if (grainCount >= 1 && (meatCount >= 1 || fishCount >= 1 || vegetableCount >= 1)) comboMultiplier = Mathf.Max(comboMultiplier, 1.5f);
        if (meatCount >= 1 && dairyCount >= 1) comboMultiplier = Mathf.Max(comboMultiplier, 1.5f);
        if (vegetableCount >= 1 && dairyCount >= 1) comboMultiplier = Mathf.Max(comboMultiplier, 2f);
    }

    if (!containsDislikedFood)
    {
        if (comboMultiplier > 1f)
        {
            happyReaction.SetActive(true);
            neutralReaction.SetActive(false);
            sadReaction.SetActive(false);
            FindObjectOfType<AudioManager>().Play("happy_sound");
        }
        else
        {
            neutralReaction.SetActive(true);
            happyReaction.SetActive(false);
            sadReaction.SetActive(false);
        }
    }

    roundPoints = Mathf.CeilToInt(roundPoints * comboMultiplier);
    totalPoints += roundPoints;

    if (proteinSlider != null) proteinSlider.value = totalProtein;
    if (carbsSlider != null) carbsSlider.value = totalCarbs;
    if (etcSlider != null) etcSlider.value = totalEtc;
    if (caloriesSlider != null) caloriesSlider.value = totalCalories;
    if (vitaminsSlider != null) vitaminsSlider.value = totalVitamins;
    if (mineralsSlider != null) mineralsSlider.value = totalMinerals;

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
    if (discardCharge > 0) 
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