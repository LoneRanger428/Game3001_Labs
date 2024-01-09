using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentMovement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;

    private Vector3 targetPosition = Vector3.zero;

    
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPosition.z = 0.0f;
        }
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed);
        LookAt2D(targetPosition);
    }

    void LookAt2D(Vector3 target)
    {
        Vector3 lookDirection = target - transform.position;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x)*Mathf.Rad2Deg;

        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    
    
}
