using UnityEngine;
using UnityEngine.SceneManagement;  // Damit wir die Szene abfragen können
using System.Collections.Generic;

[System.Serializable]
public class Persona
{
    public int level;  // Das Level als Integer (Level 1, 2, usw.)
    public float pProtein;
    public float pCarbs;
    public float pCalories;
    public List<string> Dislikes;
    public int maxScore;  // Neues Feld für den maximalen Punktestand

    // Konstruktor
    public Persona(int level, float protein, float carbs, float calories, List<string> dislikes, int maxScore)
    {
        this.level = level;
        pProtein = protein;
        pCarbs = carbs;
        pCalories = calories;
        Dislikes = dislikes;
        this.maxScore = maxScore;  // Initialisierung des maxScore
    }
}

public class PersonaManager : MonoBehaviour
{
    // Liste von Personas, die im Inspector hinzugefügt werden kann
    public List<Persona> personas;

    public Persona currentPersona;

    private void Start()
    {
        // Finde die passende Persona basierend auf dem Szenennamen
        AssignPersonaToLevel();
        
        // Gib die aktuelle Persona aus (kannst du nach Belieben weiterverwenden)
        if (currentPersona != null)
        {
            Debug.Log($"Aktuelle Persona für Level {currentPersona.level}:");
            Debug.Log($"Protein: {currentPersona.pProtein}");
            Debug.Log($"Carbs: {currentPersona.pCarbs}");
            Debug.Log($"Calories: {currentPersona.pCalories}");
            Debug.Log($"Dislikes: {string.Join(", ", currentPersona.Dislikes)}");
            Debug.Log($"Max Score: {currentPersona.maxScore}");  // Ausgabe des maxScores
        }
        else
        {
            Debug.LogWarning("Keine Persona für das aktuelle Level gefunden!");
        }
    }

    // Diese Methode weist die Persona für das aktuelle Level zu
    private void AssignPersonaToLevel()
    {
        // Hole den Szenennamen
        string currentSceneName = SceneManager.GetActiveScene().name;

        // Bestimme das Level aus dem Szenennamen (z. B. Level1 -> Level 1)
        int levelNumber = 0;
        if (currentSceneName.StartsWith("Level"))
        {
            string levelString = currentSceneName.Substring(5);  // "Level1" -> "1"
            if (int.TryParse(levelString, out levelNumber))
            {
                // Finde die Persona mit diesem Level
                currentPersona = personas.Find(p => p.level == levelNumber);
            }
        }

        if (currentPersona == null)
        {
            Debug.LogWarning("Keine Persona für das aktuelle Level gefunden!");
        }
    }
}