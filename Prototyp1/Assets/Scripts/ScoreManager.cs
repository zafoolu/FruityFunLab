using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Für TextMeshPro


public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI score;
    public TextMeshProUGUI maxScore;


    


    void Start(){
        
    }



    void Update(){


        PersonaManager personaManager = FindObjectOfType<PersonaManager>();
        maxScore.text = personaManager.currentPersona.maxScore.ToString();


        score.text = Draggable.totalPoints.ToString();
    }

}
