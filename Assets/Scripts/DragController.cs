using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragController : MonoBehaviour
{
        Vector3 offset;
        Collider2D collider2d;
        public string destinationTag = "DropArea";
        public DragScenerios scenerio;
    Vector3 startpos;
        void Awake()
        {
            collider2d = GetComponent<Collider2D>();
        startpos = transform.position;
        }

        void OnMouseDown()
        {
            offset = transform.position - MouseWorldPosition();
        }

        void OnMouseDrag()
        {
            transform.position = MouseWorldPosition() + offset;
            GameObject.FindAnyObjectByType<DragManager>().OnFingueMove();
        }

        void OnMouseUp()
        {
            collider2d.enabled = false;
            var rayOrigin = Camera.main.transform.position;
            var rayDirection = MouseWorldPosition() - Camera.main.transform.position;
            RaycastHit2D hitInfo;
            if (hitInfo = Physics2D.Raycast(rayOrigin, rayDirection))
            {
                if (hitInfo.transform.tag == destinationTag)
                {
                    transform.position = hitInfo.transform.position + new Vector3(0, 0, -0.01f);
                    GameObject.FindAnyObjectByType<DragManager>().OnDragCompleted(scenerio,true);
                }
            }
            collider2d.enabled = true;
        }

        Vector3 MouseWorldPosition()
        {
            var mouseScreenPos = Input.mousePosition;
            mouseScreenPos.z = Camera.main.WorldToScreenPoint(transform.position).z;
            return Camera.main.ScreenToWorldPoint(mouseScreenPos);
        }
    public void Reset()
    {
        transform.position = startpos;
    }

}
[Serializable]
public enum DragScenerios
{
    left,right,up,down
}