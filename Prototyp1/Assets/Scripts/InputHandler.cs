using UnityEngine;

public class InputHandler : MonoBehaviour
{
    void Update()
    {
        // Überprüft auf beliebige Tasten- oder Mausklicks
        if (Input.anyKeyDown || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            Draggable.ResetZoomedCard();
        }

        // Überprüft Touch-Eingaben
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                Draggable.ResetZoomedCard();
            }
        }
    }
}