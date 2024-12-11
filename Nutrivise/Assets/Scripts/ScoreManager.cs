using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Für TextMeshPro
using UnityEngine.UI; // Für Slider


public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI score;
    public TextMeshProUGUI maxScore;

    public Slider proteinSlider;
    public Slider carbsSlider;
    public Slider caloriesSlider;

    void Start()
    {
        
        PersonaManager personaManager = FindObjectOfType<PersonaManager>();

    
    }

    void Update()
    {
        
    PersonaManager personaManager = FindObjectOfType<PersonaManager>();
    if (personaManager != null)
        {
            proteinSlider.maxValue = personaManager.currentPersona.pProtein;
            carbsSlider.maxValue = personaManager.currentPersona.pCarbs;
            caloriesSlider.maxValue = personaManager.currentPersona.pCalories;
        }
        
        

        maxScore.text = personaManager.currentPersona.maxScore.ToString();



        score.text = Draggable.totalPoints.ToString();
    }
}