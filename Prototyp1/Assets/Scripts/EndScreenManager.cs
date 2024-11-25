using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EndscreenManager : MonoBehaviour
{
    // Dein Endscreen Panel
    public GameObject endscreenPanel;

    // Game Over Panel
    public GameObject gameOverPanel;

    // Textfelder für die Gesamtwerte
    public TextMeshProUGUI totalProteinText;
    public TextMeshProUGUI totalCarbsText;
    public TextMeshProUGUI totalEtcText;
    public TextMeshProUGUI totalCaloriesText;
    public TextMeshProUGUI totalVitaminsText;
    public TextMeshProUGUI totalMineralsText;
    public TextMeshProUGUI totalPointsText;
    public TextMeshProUGUI moneyText;

    // Referenz zum Hand-Panel (zum Zählen der Karten)
    public GameObject handPanel;

    // Neue Variablen für maximale Punktzahlen auf jedem Level
    public int maxPointsLevel1 = 60;
    public int maxPointsLevel2 = 70;
    public int maxPointsLevel3 = 80;

    // Referenz zum DeckManager Skript
    public DeckManager deckManager;

    private bool isGameOver = false;

    void Awake()
    {
        // Alle Gesamtwerte zurücksetzen
        Draggable.totalProtein = 0;
        Draggable.totalCarbs = 0;
        Draggable.totalEtc = 0;
        Draggable.totalCalories = 0;
        Draggable.totalVitamins = 0;
        Draggable.totalMinerals = 0;
        Draggable.totalPoints = 0;

        // Endscreen Panel und Game Over Panel ausblenden
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

        // Setze die maximalen Punktzahlen für das aktuelle Level
        int levelTargetPoints = GetLevelTargetPoints(SceneManager.GetActiveScene().name);

        // Wenn die Gesamtpunkte erreicht wurden, zeige den Endscreen an
        if (!isGameOver && Draggable.totalPoints >= levelTargetPoints)
        {
            ShowEndscreen();
        }
    }

    // Methode zur Rückgabe des Zielwerts für Punkte pro Level
    int GetLevelTargetPoints(string sceneName)
    {
        switch (sceneName)
        {
            case "Level1":
                return maxPointsLevel1; // Zielpunkte für Level 1
            case "Level2":
                return maxPointsLevel2; // Zielpunkte für Level 2
            case "Level3":
                return maxPointsLevel3; // Zielpunkte für Level 3
            default:
                return 60; // Standardwert
        }
    }

    // Methode zur Überprüfung, ob das Game Over erreicht wurde
    public void CheckGameOver()
    {
        int levelTargetPoints = GetLevelTargetPoints(SceneManager.GetActiveScene().name);

        // Überprüfe, ob die Anzahl der Karten im Hand-Panel 0 ist, der ClickCount 9 ist und die Punktzahl nicht passt
        bool isHandEmpty = handPanel.transform.childCount == 0;  // Überprüfen, ob keine Karten mehr im Hand-Panel sind
        bool isClickCountNine = deckManager.clickCount == 9; // Überprüfen, ob der Click Count 9 ist
        bool isScoreNotMatching = Draggable.totalPoints < levelTargetPoints;  // Überprüfen, ob die Punktzahl nicht ausreicht

        if (isHandEmpty && isClickCountNine && isScoreNotMatching)
        {
            ShowGameOver();
        }
    }

    // Game Over anzeigen
    public void ShowGameOver()
    {
        isGameOver = true;

        // Game Over Panel anzeigen
        gameOverPanel.SetActive(true);

        Debug.Log("Game Over wurde angezeigt.");
    }

    // Endscreen anzeigen
    public void ShowEndscreen()
    {
        Draggable.money += 6; // Bonus bei Levelabschluss
        isGameOver = true;

        // Textfelder für die Gesamtwerte und Punkte anzeigen
        UpdateTextWithColor(totalProteinText, Draggable.totalProtein / 10f, 
            new int[] { 0, 15, 25, 35, 45 }, new Color[] { Color.red, Color.yellow, Color.green, Color.yellow, Color.red });

        UpdateTextWithColor(totalCarbsText, Draggable.totalCarbs / 10f, 
            new int[] { 0, 225, 325, 375, 475 }, new Color[] { Color.red, Color.yellow, Color.green, Color.yellow, Color.red });

        UpdateTextWithColorForCalories(totalCaloriesText, Draggable.totalCalories, 
            new int[] { 0, 300, 700, 900, 1300 }, new Color[] { Color.red, Color.yellow, Color.green, Color.yellow, Color.red });

        UpdateTextWithColor(totalEtcText, Draggable.totalEtc / 10f, 
            new int[] { 0, 15, 25, 35, 45 }, new Color[] { Color.red, Color.yellow, Color.green, Color.yellow, Color.red });

        UpdateTextWithColorForVitamins(totalVitaminsText, Draggable.totalVitamins, 
            new int[] { 0, 5, 10, 12, 16 }, new Color[] { Color.red, Color.yellow, Color.green, Color.yellow, Color.red });

        UpdateTextWithColorForMinerals(totalMineralsText, Draggable.totalMinerals, 
            new int[] { 0, 5, 10, 12, 16 }, new Color[] { Color.red, Color.yellow, Color.green, Color.yellow, Color.red });

        totalPointsText.text = $"{Draggable.totalPoints}";
        endscreenPanel.SetActive(true); // Endscreen Panel anzeigen
    }

    // Text-Update für Protein, Carbs etc.
    void UpdateTextWithColor(TextMeshProUGUI text, float value, int[] thresholds, Color[] colors)
    {
        text.text = $"{value}"; // Zeige den Wert
        for (int i = 0; i < thresholds.Length; i++)
        {
            if (value >= thresholds[i])
            {
                text.color = colors[i];
            }
        }
    }

    // Gleiches für Calories, Vitamins und Minerals (mit anderen Schwellenwerten)
    void UpdateTextWithColorForCalories(TextMeshProUGUI text, float value, int[] thresholds, Color[] colors)
    {
        UpdateTextWithColor(text, value, thresholds, colors);
    }

    void UpdateTextWithColorForVitamins(TextMeshProUGUI text, float value, int[] thresholds, Color[] colors)
    {
        UpdateTextWithColor(text, value, thresholds, colors);
    }

    void UpdateTextWithColorForMinerals(TextMeshProUGUI text, float value, int[] thresholds, Color[] colors)
    {
        UpdateTextWithColor(text, value, thresholds, colors);
    }

    // Methode zum Laden der nächsten Szene
    public void LoadNextLevel()
    {
        if (endscreenPanel != null)
        {
            endscreenPanel.SetActive(false);
        }

        Scene currentScene = SceneManager.GetActiveScene();
        string currentSceneName = currentScene.name;

        switch (currentSceneName)
        {
            case "Level1":
                SceneManager.LoadScene("Level2");
                break;
            case "Level2":
                SceneManager.LoadScene("Level3");
                break;
            default:
                Debug.Log("Keine nächste Szene definiert!");
                break;
        }
    }

    // Methode, die nach dem Laden einer neuen Szene aufgerufen wird
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
}