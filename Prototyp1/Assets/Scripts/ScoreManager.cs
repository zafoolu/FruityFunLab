using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Für TextMeshPro


public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI score;





    void Update(){
        score.text = Draggable.totalPoints.ToString();
    }

}
