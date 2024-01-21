using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickDragScript : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        //First we check if player clicked on screen
        if(Input.GetMouseButtonDown(0))
        {
            //Raycast to check if the mouse is over a collider
            RaycasterHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            
            if(hit.collider != null) //we click to a object with collider
            {
                //Check if the clicked Gameobject has a rigidbody2d
                Rigidbody2D rb2d = hit.collider.GetComponent<Rigidbody2D>(); //try to get rigidbody2d from the gameobject from its collider

                if(rb2d != null)
                {
                    //Start dragging only if no object is currently being targeted
                    isDragging = true;
                    currentDraggerObject = rb2d;

                    offset = rb2d.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                }
            }
        }
        else if (Input.getMouseButtonup(0))
        {
            //Stop dragging
            isDragging = false;
            currentDraggerObject = null;
        }


        //Drag object according to mouse
        if(isDragging && currentDraggerObject != null)
        {
            //Move the dragged GameObject based on the mouse position
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentDraggingObject.MovePosition(mousePosition + offset);
        }
        
    }
}
