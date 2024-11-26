using UnityEngine;

public class InputHandler : MonoBehaviour
{
    // Referenz zu deinem Tutorial-Screen-GameObject
    public GameObject tutorialScreen;
    private bool isTutorialActive;

    void Awake()
    {
        // Wenn es ein Tutorial-Screen-GameObject gibt, aktiviere es und setze das Tutorial als aktiv
        if (tutorialScreen != null)
        {
            tutorialScreen.SetActive(true);
            isTutorialActive = true;
        }
        else
        {
            // Wenn kein Tutorial-Screen vorhanden ist, ist nichts zu tun
            isTutorialActive = false;
            return;
        }
    }

    void Update()
    {
        // Nur Eingaben prüfen, wenn das Tutorial aktiv ist
        if (isTutorialActive)
        {
            // Überprüft auf beliebige Tasten- oder Mausklicks
            if (Input.anyKeyDown || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                DeactivateTutorial();
            }

            // Überprüft Touch-Eingaben
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

    // Funktion zum Deaktivieren des Tutorials
    private void DeactivateTutorial()
    {
        if (tutorialScreen != null)
        {
            tutorialScreen.SetActive(false);
        }
        isTutorialActive = false;
    }
}