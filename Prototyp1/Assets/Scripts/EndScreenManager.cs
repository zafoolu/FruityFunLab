using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EndscreenManager : MonoBehaviour
{
    
    public GameObject endscreenPanel;

    
    public GameObject gameOverPanel;

    
    public TextMeshProUGUI totalProteinText;
    public TextMeshProUGUI totalCarbsText;
    public TextMeshProUGUI totalEtcText;
    public TextMeshProUGUI totalCaloriesText;
    public TextMeshProUGUI totalVitaminsText;
    public TextMeshProUGUI totalMineralsText;
    public TextMeshProUGUI totalPointsText;
    public TextMeshProUGUI moneyText;

    
    public GameObject handPanel;

    
    public int maxPointsLevel1 = 60;
    public int maxPointsLevel2 = 70;
    public int maxPointsLevel3 = 80;

    
    public DeckManager deckManager;

    private bool isGameOver = false;

    

    void Awake()
    {
        
        Draggable.totalProtein = 0;
        Draggable.totalCarbs = 0;
        Draggable.totalEtc = 0;
        Draggable.totalCalories = 0;
        Draggable.totalVitamins = 0;
        Draggable.totalMinerals = 0;
        Draggable.totalPoints = 0;

        
        endscreenPanel.SetActive(false);
        gameOverPanel.SetActive(false);

        Debug.Log("Alle total-Werte wurden zurückgesetzt.");
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Update()
    {
        moneyText.text = $"{Draggable.money}";

        
        int levelTargetPoints = GetLevelTargetPoints(SceneManager.GetActiveScene().name);

        
        if (!isGameOver && Draggable.totalPoints >= levelTargetPoints)
        {
            ShowEndscreen();
        }
    }

    
    int GetLevelTargetPoints(string sceneName)
    {

        PersonaManager personaManager = FindObjectOfType<PersonaManager>();
        switch (sceneName)
        {
            case "Level1":
                return personaManager.currentPersona.maxScore; // Zielpunkte für Level 1
            case "Level2":
                return personaManager.currentPersona.maxScore; // Zielpunkte für Level 2
            case "Level3":
                return personaManager.currentPersona.maxScore;; // Zielpunkte für Level 3
            default:
                return 60; // Standardwert
        }
    }

    
    public void CheckGameOver()
    {
        int levelTargetPoints = GetLevelTargetPoints(SceneManager.GetActiveScene().name);

    
        bool isHandEmpty = handPanel.transform.childCount == 0;  
        bool isClickCountNine = deckManager.clickCount == deckManager.maxCards; 
        bool isScoreNotMatching = Draggable.totalPoints < levelTargetPoints;  

        if (isHandEmpty && isClickCountNine && isScoreNotMatching)
        {
            ShowGameOver();
        }
    }

    
    public void ShowGameOver()
    {
        isGameOver = true;
        gameOverPanel.SetActive(true);

        Debug.Log("Game Over wurde angezeigt.");
    }

    public void ShowEndscreen()
    {
        Draggable.money += 6; 
        isGameOver = true;

        
        UpdateTextWithColor(totalProteinText, "TOTAL PROTEIN", Draggable.totalProtein / 10f, 
            new int[] { 0, 15, 25, 35, 45 }, new Color[] { Color.red, Color.yellow, Color.green, Color.yellow, Color.red });

        UpdateTextWithColor(totalCarbsText, "TOTAL CARBS", Draggable.totalCarbs / 10f, 
            new int[] { 0, 225, 325, 375, 475 }, new Color[] { Color.red, Color.yellow, Color.green, Color.yellow, Color.red });

        UpdateTextWithColor(totalCaloriesText, "TOTAL CALORIES", Draggable.totalCalories, 
            new int[] { 0, 300, 700, 900, 1300 }, new Color[] { Color.red, Color.yellow, Color.green, Color.yellow, Color.red });

        UpdateTextWithColor(totalEtcText, "TOTAL ETC", Draggable.totalEtc / 10f, 
            new int[] { 0, 15, 25, 35, 45 }, new Color[] { Color.red, Color.yellow, Color.green, Color.yellow, Color.red });

        UpdateTextWithColor(totalVitaminsText, "TOTAL VITAMINS", Draggable.totalVitamins, 
            new int[] { 0, 5, 10, 12, 16 }, new Color[] { Color.red, Color.yellow, Color.green, Color.yellow, Color.red });

        UpdateTextWithColor(totalMineralsText, "TOTAL MINERALS", Draggable.totalMinerals, 
            new int[] { 0, 5, 10, 12, 16 }, new Color[] { Color.red, Color.yellow, Color.green, Color.yellow, Color.red });

        totalPointsText.text = $"TOTAL POINTS: {Draggable.totalPoints}";
        endscreenPanel.SetActive(true); // Endscreen Panel anzeigen
    }

    
    void UpdateTextWithColor(TextMeshProUGUI text, string prefix, float value, int[] thresholds, Color[] colors)
    {
        text.text = $"{prefix}: {value}"; 
        for (int i = 0; i < thresholds.Length; i++)
        {
            if (value >= thresholds[i])
            {
                text.color = colors[i];
            }
        }
    }

    
    void UpdateTextWithColorForCalories(TextMeshProUGUI text, string prefix, float value, int[] thresholds, Color[] colors)
    {
        UpdateTextWithColor(text, prefix, value, thresholds, colors);
    }

    void UpdateTextWithColorForVitamins(TextMeshProUGUI text, string prefix, float value, int[] thresholds, Color[] colors)
    {
        UpdateTextWithColor(text, prefix, value, thresholds, colors);
    }

    void UpdateTextWithColorForMinerals(TextMeshProUGUI text, string prefix, float value, int[] thresholds, Color[] colors)
    {
        UpdateTextWithColor(text, prefix, value, thresholds, colors);
    }

    
    public void LoadNextLevel()
    {
        if (endscreenPanel != null)
        {
            endscreenPanel.SetActive(false);
        }

        // Aktuelle Szene abrufen
        Scene currentScene = SceneManager.GetActiveScene();
        string currentSceneName = currentScene.name;

        // Prüfen, ob die aktuelle Szene ein Level ist
        if (currentSceneName.StartsWith("Level"))
        {
            int currentLevelNumber;
            if (int.TryParse(currentSceneName.Substring(5), out currentLevelNumber))
            {
                // Nächsten Levelnamen berechnen
                string nextSceneName = "Level" + (currentLevelNumber + 1);

                // Prüfen, ob die nächste Szene existiert
                if (SceneExists(nextSceneName))
                {
                    SceneManager.LoadScene(nextSceneName);
                }
                else
                {
                    Debug.Log("Keine nächste Szene definiert!");
                }
            }
            else
            {
                Debug.LogError("Die aktuelle Szene hat keinen gültigen Levelnamen!");
            }
        }
        else
        {
            Debug.Log("Die aktuelle Szene ist kein Level!");
        }
    }

    private bool SceneExists(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            string scene = System.IO.Path.GetFileNameWithoutExtension(path);
            if (scene == sceneName)
            {
                return true;
            }
        }
        return false;
    }

    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (endscreenPanel != null)
        {
            endscreenPanel.SetActive(false);
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        isGameOver = false;

        Debug.Log($"Szene {scene.name} wurde geladen. Endscreen zurückgesetzt.");
    }

    public void GoBack()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void TryAgain()
    {
        SceneManager.LoadScene("Level1");
    }
}