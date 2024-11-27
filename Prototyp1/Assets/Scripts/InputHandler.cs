using UnityEngine;

public class InputHandler : MonoBehaviour
{
    
    public GameObject tutorialScreen;
    private bool isTutorialActive;

    void Awake()
    {
        if (tutorialScreen != null)
        {
            tutorialScreen.SetActive(true);
            isTutorialActive = true;
        }
        else
        {
            isTutorialActive = false;
            return;
        }
    }

    void Update()
    {
        
        if (isTutorialActive)
        {
        
            if (Input.anyKeyDown || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                DeactivateTutorial();
            }

            
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    DeactivateTutorial();
                }
            }
        }
    }

    
    private void DeactivateTutorial()
    {
        if (tutorialScreen != null)
        {
            tutorialScreen.SetActive(false);
        }
        isTutorialActive = false;
    }
}