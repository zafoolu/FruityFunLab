using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

[System.Serializable]
public class Persona
{
    public int level;
    public float pProtein;
    public float pCarbs;
    public float pCalories;
    public List<string> Dislikes;
    public int maxScore;

    public Persona(int level, float protein, float carbs, float calories, List<string> dislikes, int maxScore)
    {
        this.level = level;
        pProtein = protein;
        pCarbs = carbs;
        pCalories = calories;
        Dislikes = dislikes;
        this.maxScore = maxScore;
    }
}

public class PersonaManager : MonoBehaviour
{
    public List<Persona> personas;

    public Persona currentPersona;

    private void Start()
    {
        AssignPersonaToLevel();
    }

    private void AssignPersonaToLevel()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        int levelNumber = 0;
        if (currentSceneName.StartsWith("Level"))
        {
            string levelString = currentSceneName.Substring(5);
            if (int.TryParse(levelString, out levelNumber))
            {
                currentPersona = personas.Find(p => p.level == levelNumber);
            }
        }

        if (currentPersona == null)
        {
            Debug.LogWarning("Keine Persona für das aktuelle Level gefunden!");
        }
    }
}