using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EndscreenManager : MonoBehaviour
{
    // Dein Endscreen Panel
    public GameObject endscreenPanel;

    // Textfelder für die Gesamtwerte
    public TextMeshProUGUI totalProteinText;
    public TextMeshProUGUI totalCarbsText;
    public TextMeshProUGUI totalEtcText;
    public TextMeshProUGUI totalCaloriesText;
    public TextMeshProUGUI totalVitaminsText;
    public TextMeshProUGUI totalMineralsText;
    public TextMeshProUGUI totalPointsText;
    public TextMeshProUGUI moneyText;

    // Neue Variablen für maximale Punktzahlen auf jedem Level
    public int maxPointsLevel1 = 60;
    public int maxPointsLevel2 = 70;
    public int maxPointsLevel3 = 80;

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

        // Endscreen Panel ausblenden
        endscreenPanel.SetActive(false);

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

        totalEtcText.text = $"Total Etc: {Draggable.totalEtc}";
        totalVitaminsText.text = $"Total Vitamins: {Draggable.totalVitamins}";
        totalMineralsText.text = $"Total Minerals: {Draggable.totalMinerals}";
        totalPointsText.text = $"Total Points: {Draggable.totalPoints}";

        // Endscreen Panel aktivieren
        endscreenPanel.SetActive(true);

        Debug.Log("Endscreen wurde angezeigt.");
    }

    // Hilfsmethode, um den Text mit Farbe basierend auf dem Wert zu aktualisieren
    private void UpdateTextWithColor(TextMeshProUGUI textField, float value, int[] thresholds, Color[] colors)
    {
        textField.text = $"{textField.text.Split(':')[0]}: {value:F2}";

        for (int i = 0; i < thresholds.Length - 1; i++)
        {
            if (value >= thresholds[i] && value < thresholds[i + 1])
            {
                textField.color = colors[i];
                return;
            }
        }

        textField.color = colors[colors.Length - 1];
    }

    // Spezielle Methode, um den Text mit Farbe für die Kalorien zu aktualisieren
    private void UpdateTextWithColorForCalories(TextMeshProUGUI textField, float value, int[] thresholds, Color[] colors)
    {
        textField.text = $"{textField.text.Split(':')[0]}: {value}";

        for (int i = 0; i < thresholds.Length - 1; i++)
        {
            if (value >= thresholds[i] && value < thresholds[i + 1])
            {
                textField.color = colors[i];
                return;
            }
        }

        textField.color = colors[colors.Length - 1];
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

        isGameOver = false;

        Debug.Log($"Szene {scene.name} wurde geladen. Endscreen zurückgesetzt.");
    }
}