using UnityEngine;
using TMPro; // Für TextMeshPro

public class EndscreenManager : MonoBehaviour
{
    public GameObject endscreenPanel; // Das gesamte Panel mit Image und Kinder-Objekten
    public TextMeshProUGUI totalProteinText;    // Text für Gesamt-Protein
    public TextMeshProUGUI totalCarbsText;      // Text für Gesamt-Kohlenhydrate
    public TextMeshProUGUI totalEtcText;        // Text für Gesamt-"Etc"
    public TextMeshProUGUI totalCaloriesText;   // Text für Gesamt-Kalorien
    public TextMeshProUGUI totalVitaminsText;   // Text für Gesamt-Vitamine
    public TextMeshProUGUI totalMineralsText;   // Text für Gesamt-Mineralien
    public TextMeshProUGUI totalPointsText;     // Text für Gesamt-Punkte (optional)
    public TextMeshProUGUI moneyText;           // Geld-Anzeige

    private bool isGameOver = false;

    void Update()
    {
        moneyText.text = $"MONEY: {Draggable.money}";

        // Überprüfe die Bedingung (Gesamtscore)
        if (!isGameOver && Draggable.totalPoints >= 60)
        {
            ShowEndscreen();
        }
    }

    public void ShowEndscreen()
    {
        Draggable.money += 6;
        isGameOver = true; // Verhindert mehrfaches Aktivieren

        // Setze die Werte in den entsprechenden Textfeldern und ändere die Farben
        UpdateTextWithColor(totalProteinText, Draggable.totalProtein / 10f, 
                            new int[] { 0, 15, 25, 35, 45 }, new Color[] { Color.red, Color.yellow, Color.green, Color.yellow, Color.red });

        UpdateTextWithColor(totalCarbsText, Draggable.totalCarbs / 10f, 
                            new int[] { 0, 225, 325, 375, 475 }, new Color[] { Color.red, Color.yellow, Color.green, Color.yellow, Color.red });

        // Kalorien ohne Teilen, aber mit Farbanpassung
        UpdateTextWithColorForCalories(totalCaloriesText, Draggable.totalCalories, 
                                       new int[] { 0, 300, 700, 900, 1300 }, new Color[] { Color.red, Color.yellow, Color.green, Color.yellow, Color.red });

        // Werte für die anderen Variablen setzen, die nicht geteilt werden müssen
        totalEtcText.text = $"Total Etc: {Draggable.totalEtc}";
        totalVitaminsText.text = $"Total Vitamins: {Draggable.totalVitamins}";
        totalMineralsText.text = $"Total Minerals: {Draggable.totalMinerals}";
        totalPointsText.text = $"Total Points: {Draggable.totalPoints}"; // Optional

        // Endscreen-Panel aktivieren
        endscreenPanel.SetActive(true); // Das gesamte Panel mit allen Kindern aktivieren

        Debug.Log("Endscreen wurde angezeigt.");
    }

    // Diese Methode setzt den Text und ändert die Farbe basierend auf den definierten Bereichen
    private void UpdateTextWithColor(TextMeshProUGUI textField, float value, int[] thresholds, Color[] colors)
    {
        // Setze den Text und formatiere ihn auf eine gewünschte Anzahl Dezimalstellen
        textField.text = $"{textField.text.Split(':')[0]}: {value:F2}"; // F2 zeigt 2 Dezimalstellen an

        // Bestimme die Farbe basierend auf den Schwellenwerten
        for (int i = 0; i < thresholds.Length - 1; i++)
        {
            if (value >= thresholds[i] && value < thresholds[i + 1])
            {
                textField.color = colors[i];
                return;
            }
        }

        // Wenn kein Bereich passt, setze den Standardwert (Farbe rot als Fallback)
        textField.color = colors[colors.Length - 1];
    }

    // Neue Methode zur Farbanpassung der Kalorien, da Kalorien nicht geteilt werden
    private void UpdateTextWithColorForCalories(TextMeshProUGUI textField, float value, int[] thresholds, Color[] colors)
    {
        // Setze den Text und formatiere ihn auf eine gewünschte Anzahl Dezimalstellen
        textField.text = $"{textField.text.Split(':')[0]}: {value}";

        // Bestimme die Farbe basierend auf den Schwellenwerten
        for (int i = 0; i < thresholds.Length - 1; i++)
        {
            if (value >= thresholds[i] && value < thresholds[i + 1])
            {
                textField.color = colors[i];
                return;
            }
        }

        // Wenn kein Bereich passt, setze den Standardwert (Farbe rot als Fallback)
        textField.color = colors[colors.Length - 1];
    }
}