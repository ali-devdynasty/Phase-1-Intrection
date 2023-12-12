using UnityEngine;

public class TouchChecker : MonoBehaviour
{
    public bool IsTouchOnSprite()
    {
        // Check for touch input
        if (Input.touchCount > 0)
        {
            // Get the first touch
            Touch touch = Input.GetTouch(0);

            // Convert touch position to world space
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

            // Raycast to check if the touch is on a GameObject with a SpriteRenderer
            RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero);

            // Check if the ray hit a collider
            if (hit.collider != null)
            {
                Debug.Log("Collider " + hit.collider.gameObject.name);
                // Check if the GameObject has a SpriteRenderer component
                SpriteRenderer spriteRenderer = hit.collider.GetComponent<SpriteRenderer>();

                // Return true if the touch is on a GameObject with a SpriteRenderer component
                return spriteRenderer != null;
            }
        }

        // Return false if no touch or not on a GameObject with a SpriteRenderer
        return false;
    }
}
