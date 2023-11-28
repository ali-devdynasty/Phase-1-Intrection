using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CheckTouchPostion : MonoBehaviour
{
    public bool IsTouchOverUI(Vector2 touchPosition)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = touchPosition;

        // Raycast using the UI event system
        GraphicRaycaster[] raycasters = FindObjectsOfType<GraphicRaycaster>();
        foreach (GraphicRaycaster raycaster in raycasters)
        {
            // Create a list to store the results of the raycast
            List<RaycastResult> results = new List<RaycastResult>();
            raycaster.Raycast(eventData, results);

            // Check if there are any results (UI elements) under the touch position
            if (results.Count > 0)
            {
                return true; // Touch is over a UI element
            }
        }

        return false; // Touch is not over any UI element
    }
}
