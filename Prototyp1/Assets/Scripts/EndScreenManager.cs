using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EndscreenManager : MonoBehaviour
{
    public GameObject endscreenPanel;
    public TextMeshProUGUI totalProteinText;
    public TextMeshProUGUI totalCarbsText;
    public TextMeshProUGUI totalEtcText;
    public TextMeshProUGUI totalCaloriesText;
    public TextMeshProUGUI totalVitaminsText;
    public TextMeshProUGUI totalMineralsText;
    public TextMeshProUGUI totalPointsText;
    public TextMeshProUGUI moneyText;

    private bool isGameOver = false;



        void Awake()
    {
         endscreenPanel.SetActive(false);
        Draggable.totalProtein = 0;
        Draggable.totalCarbs = 0;
        Draggable.totalEtc = 0;
        Draggable.totalCalories = 0;
        Draggable.totalVitamins = 0;
        Draggable.totalMinerals = 0;
        Draggable.totalPoints = 0;

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

        if (!isGameOver && Draggable.totalPoints >= 60)
        {
            ShowEndscreen();
        }
    }

    public void ShowEndscreen()
    {
        Draggable.money += 6;
        isGameOver = true;

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

        endscreenPanel.SetActive(true);

        Debug.Log("Endscreen wurde angezeigt.");
    }

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