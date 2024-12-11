using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public Animator animator; // Dein Animator
    public string obstSalatAnimationTrigger = "ObstSalatTrigger"; // Der Trigger für die Obstsalat-Animation

    public void TriggerObstSalatAnimation()
    {
        if (animator != null)
        {
            // Die Animation starten
            animator.SetTrigger(obstSalatAnimationTrigger);
        }
    }
}