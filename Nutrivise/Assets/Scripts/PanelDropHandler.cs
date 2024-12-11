using UnityEngine;
using UnityEngine.EventSystems;

public class PanelDropHandler : MonoBehaviour, IDropHandler
{
    public float tolerance = 150f; 

    public void OnDrop(PointerEventData eventData)
    {
        
        GameObject draggedObject = eventData.pointerDrag;
        if (draggedObject == null) return;

        
        Draggable draggedCard = draggedObject.GetComponent<Draggable>();
        if (draggedCard == null) return;

        
        Transform panelTransform = transform;

        
        Vector3 localMousePosition = panelTransform.InverseTransformPoint(eventData.position);

        
        int closestIndex = panelTransform.childCount; 
        float closestDistance = float.MaxValue;

        
        for (int i = 0; i < panelTransform.childCount; i++)
        {
            Transform child = panelTransform.GetChild(i);

            
            if (child == draggedCard.transform) continue;

            
            Vector3 localChildPosition = panelTransform.InverseTransformPoint(child.position);
            float distance = Mathf.Abs(localMousePosition.x - localChildPosition.x);

          
            if (distance < closestDistance && distance < tolerance)
            {
                closestDistance = distance;
                closestIndex = i;
            }
        }

        
        draggedCard.transform.SetParent(panelTransform);

        if (closestIndex < panelTransform.childCount)
        {
            
            draggedCard.transform.SetSiblingIndex(closestIndex);
        }
        else
        {
            
            draggedCard.transform.SetSiblingIndex(panelTransform.childCount);
        }
    }
}