using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowDestroyer : MonoBehaviour
{
    private void Update()
    {
        // Check if the arrow is out of the screen
        if (IsOutOfScreen())
        {
            // Destroy the arrow
            Destroy(gameObject);
        }
    }

    private bool IsOutOfScreen()
    {
        Vector3 arrowScreenPosition = Camera.main.WorldToScreenPoint(transform.position);
        return arrowScreenPosition.x < 0 || arrowScreenPosition.x > Screen.width || arrowScreenPosition.y < 0 || arrowScreenPosition.y > Screen.height;
    }

}
