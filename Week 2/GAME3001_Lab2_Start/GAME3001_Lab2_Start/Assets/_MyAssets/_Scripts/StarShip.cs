using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarShip : AgentObject
{
    [SerializeField] float movementSpeed;
    [SerializeField] float rotationSpeed;
    bool _resetThis = false;
    Vector2 _tempPosition;

    Rigidbody2D rb;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        Debug.Log("Starting Starship!");
        rb = GetComponent<Rigidbody2D>();
        _tempPosition = transform.position; 
    }

    // Update is called once per frame
    void Update()
    {
        if(TargetPosition != null)
        {
            Seek();
            SeekForward();
        }
    }

    private void Seek()
    {
        //Calculate desired velocity using kinematic seek equation.
        Vector2 desiredVelocity = (TargetPosition - transform.position).normalized*movementSpeed;

        //Calculating the steering force
        //Check current velocity and only apply for difference between desired velocity and current one;
        Vector2 steeringForce = desiredVelocity - rb.velocity;

        //Apply the steering force to the agent
        rb.AddForce(steeringForce);
    }

    private void SeekForward() //Always move forwrd while rotate to the target
    {
        //Calculate direction to the target
        Vector2 directionToTarget = (TargetPosition - transform.position).normalized;

        //Calculate the angle to rotate towards the target
        float targetAngle = Mathf.Atan2(directionToTarget.y , directionToTarget.x)*Mathf.Rad2Deg;

        //Smoothly rotate towards the target
        float angleDifference = Mathf.DeltaAngle(targetAngle, transform.eulerAngles.z);
        float rotationStep = rotationSpeed*Time.deltaTime;
        float rotationAmount = Mathf.Clamp(angleDifference, -rotationStep, rotationStep);

        transform.Rotate(Vector3.forward, rotationAmount);

        //Move along the forward sector 
        rb.velocity = transform.up * movementSpeed;
    }

    public void resetAll()
    {
        rb.velocity = Vector2.zero;
        transform.rotation = Quaternion.identity;
        _resetThis = true;
        transform.position = _tempPosition;

    }
}
