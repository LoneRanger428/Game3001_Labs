using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickDragScript : MonoBehaviour
{
    private bool isDragging = false;
    private Rigidbody2D currentDraggingObject;
    private Vector2 offset;
    // Update is called once per frame
    void Update()
    {
        //first we check if we clicked on screen

        if (Input.GetMouseButtonDown(0))
        {
            //Raycast to check if the mouse  is over a  collider 
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null)
            {
                Rigidbody2D rb2d = hit.collider.GetComponent<Rigidbody2D>();

                if (rb2d != null)
                {
                    isDragging = true;
                    currentDraggingObject = rb2d;

                    offset = rb2d.transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                }
            }
            else if (Input.GetMouseButtonDown(0))
            {
                isDragging = false;
                currentDraggingObject = null;
            }
        }
        if (isDragging && currentDraggingObject != null)
        {
            //Move the dragged Gameobject based on the position
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentDraggingObject.MovePosition(mousePosition + offset);
        }
    }
}
