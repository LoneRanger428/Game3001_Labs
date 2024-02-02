using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float enemySpeed;
    [SerializeField] private float enemyRotation;
    [SerializeField] private float screenBorder;
    [SerializeField] Transform enemyTarget;
    [SerializeField] GameObject _obstacle;
    [SerializeField] GameObject Agent;
    [SerializeField] float whiskerLength;
    [SerializeField] float whiskerAngle;
    [SerializeField] float avoidanceWeight;

    private Rigidbody2D _rigidBody;
    private Camera _camera;
    private float _directionChangeCooldown;
    private Vector2 _targetDirection;
    bool _resetThis = false;
    Vector2 _tempPosition;
    private bool obstacleSpawned = false;
    private GameObject spawnedObstacle;
    [SerializeField] 
    private GameObject spawnedAgent;
    bool agentSpawned = false;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _camera = Camera.main;
        _targetDirection = transform.up;
        if (spawnedAgent != null)
        {
            TargetPosition = spawnedAgent.transform.position;
        }
        else
        {
            TargetPosition = enemyTarget.transform.position;
        }
        //TargetPosition = spawnedAgent.transform.position;
        _tempPosition = transform.position;
        
    }

    private void Update()
    {
        UpdateTargetDirection();
        RotateTowardsTarget();
        SetVelocity();
        if (obstacleSpawned == true)
        {
            ObstacleAvoid();
        }       
    }

    private void UpdateTargetDirection()
    {
        HandleEnemyOffScreen();
        RandomDirectionChangeHandler();
        if(Input.GetKey(KeyCode.Alpha1))
        {
            if (!agentSpawned)
            {
                spawnedAgent = Instantiate(Agent, ObstacleSpawn(), Quaternion.identity);
                agentSpawned = true;
            }
            PlayerTargetHandler();
        }
        else if(Input.GetKey(KeyCode.Alpha2))
        {
            TargetFlee(); 
        }
        else if(Input.GetKey(KeyCode.Alpha3))
        {
            resetAll();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) && !obstacleSpawned)
        {
            PlayerTargetHandler();
            spawnedObstacle = Instantiate(_obstacle, ObstacleSpawn(), Quaternion.identity);
            obstacleSpawned = true;
        }
        else if (Input.GetKey(KeyCode.Alpha5))
        {
            RemoveAll();
            resetAll();
            obstacleSpawned = false;
            agentSpawned = false;
        }
        
    }

    public Vector3 TargetPosition
    {
        get { return spawnedAgent.transform.position; }
        set { spawnedAgent.transform.position = value; }
    }

    private Vector3 ObstacleSpawn()
    {
        float randomX = Random.Range(Camera.main.transform.position.x - 5f, Camera.main.transform.position.x + 5f);
        float randomY = Random.Range(Camera.main.transform.position.y - 2f, Camera.main.transform.position.y + 2f);
        Vector3 spawnPosition = new Vector3(randomX, randomY, 0);
        return spawnPosition;
    }

    private void RandomDirectionChangeHandler()
    {
        _directionChangeCooldown -= Time.deltaTime;

        if (_directionChangeCooldown <= 0)
        {
            float angleChange = Random.Range(-90f, 90f);
            Quaternion rotation = Quaternion.AngleAxis(angleChange, transform.forward);
            _targetDirection = rotation * _targetDirection;

            _directionChangeCooldown = Random.Range(1f, 5f);
        }
    }

    private void PlayerTargetHandler()
    {
      
        _targetDirection = (TargetPosition - transform.position).normalized;
        Vector2 desiredVelocity = (TargetPosition - transform.position).normalized * enemySpeed;
        Vector2 steeringForce = desiredVelocity - _rigidBody.velocity;
        _rigidBody.AddForce(steeringForce);
       
    }

    private void TargetFlee()
    {
        _targetDirection = -(TargetPosition - transform.position).normalized;
        Vector2 desiredVelocity = (TargetPosition - transform.position).normalized * enemySpeed;
        Vector2 steeringForce = desiredVelocity - _rigidBody.velocity;
        _rigidBody.AddForce(steeringForce);
    }

    private void ObstacleAvoid()
    {
        // Cast whiskers to detect obstacles.
        bool hitLeft = castWhiskers(whiskerAngle, Color.red);
        bool hitRight = castWhiskers((-whiskerAngle), Color.blue);


        // Adjust rotation based on detected obstacles.
        if (hitLeft)
        {
            //rotate clockwise
            RotateClockWise();
        }
        if (hitRight && !hitLeft)
        {
            //rotate counterclockwise
            RotateCounterClockWise();
        }
    }

    private void RotateClockWise()
    {
        //Rotate clockwise based on rotate speed
        transform.Rotate(Vector3.forward, -enemyRotation * avoidanceWeight * Time.deltaTime);
    }
    private void RotateCounterClockWise()
    {
        transform.Rotate(Vector3.forward, enemyRotation * avoidanceWeight * Time.deltaTime);
    }
    private bool castWhiskers(float angle, Color color)
    {
        bool hitResult = false;
        Color rayColor = color;
        //calculate direction of whiskers
        Vector2 directionToWhiskers = Quaternion.Euler(0, 0, angle) * transform.up;

        //cast a ray in the whisker direction.
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToWhiskers, whiskerLength);

        //check if the ray is an obstacle
        if (hit.collider != null)
        {
            Debug.Log("Obstacle detected");
            rayColor = Color.green;
            hitResult = true;
        }

        Debug.DrawRay(transform.position, directionToWhiskers * whiskerLength, Color.magenta);

        return hitResult;
    }
    private void HandleEnemyOffScreen()
    {
        Vector2 screenPosition = _camera.WorldToScreenPoint(transform.position);

        if ((screenPosition.x < -100 && _targetDirection.x < 0) ||
            (screenPosition.x > _camera.pixelWidth + 100 && _targetDirection.x > 0))
        {
            _targetDirection = new Vector2(-_targetDirection.x, _targetDirection.y);
        }

        if ((screenPosition.y < -100 && _targetDirection.y < 0) ||
            (screenPosition.y > _camera.pixelHeight + 100 && _targetDirection.y > 0))
        {
            _targetDirection = new Vector2(_targetDirection.x, -_targetDirection.y);
        }
    }

    private void RotateTowardsTarget()
    {
        float targetAngle = Mathf.Atan2(_targetDirection.y, _targetDirection.x) * Mathf.Rad2Deg + 90.0f; // Note the +90 when converting from Radians.

        // Smoothly rotate towards the target.
        float angleDifference = Mathf.DeltaAngle(targetAngle, transform.eulerAngles.z);
        float rotationStep = enemyRotation * Time.deltaTime;
        float rotationAmount = Mathf.Clamp(angleDifference, -rotationStep, rotationStep);
        transform.Rotate(Vector3.forward, rotationAmount);
        //Quaternion targetRotation = Quaternion.LookRotation(transform.forward, _targetDirection);
        //Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, enemyRotation * Time.deltaTime);

        //_rigidBody.SetRotation(rotation);
    }

    private void SetVelocity()
    {
        _rigidBody.velocity = transform.up * enemySpeed;
    }

    public void RemoveAll()
    {
        if (spawnedObstacle != null)
        {
            Destroy(spawnedObstacle);
            obstacleSpawned = false; // Reset the flag after removing the obstacle
        }

        if (Agent != null)
        {
            Destroy(spawnedAgent);
            agentSpawned = false; // Reset the flag after removing the obstacle
        }
    }
    public void resetAll()
    {
        _rigidBody.velocity = Vector2.zero;
        transform.rotation = Quaternion.identity;
        _resetThis = true;
        transform.position = _tempPosition;

    }
}
