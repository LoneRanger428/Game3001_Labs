using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTorpedoScript : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    private Vector2 directionToTarget;
    private Vector2 vectorToTarget;
  
    void Update()
    {
        transform.Translate(vectorToTarget.x, vectorToTarget.y, 0f);
    }
    public void LockOnTarget(Transform target)
    {
        directionToTarget = (target.position - transform.position).normalized;  // why do we normalized?
        vectorToTarget = directionToTarget * moveSpeed * Time.deltaTime;
    }
}
