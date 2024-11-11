using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject happyReaction;
    public GameObject sadReaction;

    public void OnPointerEnter(PointerEventData eventData) { }

    public void OnPointerExit(PointerEventData eventData) { }

    public void OnDrop(PointerEventData eventData)
    {
        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();

        if (d != null)
        {
            if (d.typeOfFood == Draggable.Food.Good)
            {
                ShowReaction(happyReaction);
            }
            else if (d.typeOfFood == Draggable.Food.Bad)
            {
                ShowReaction(sadReaction);
            }
            d.parenToReturnTo = this.transform;
        }
    }

    private void ShowReaction(GameObject reactionObject)
    {
        if (reactionObject != null)
        {
            reactionObject.SetActive(true);
            StartCoroutine(HideReaction(reactionObject));
        }
    }

    private IEnumerator HideReaction(GameObject reactionObject)
    {
        yield return new WaitForSeconds(2f);
        reactionObject.SetActive(false);
    }
}