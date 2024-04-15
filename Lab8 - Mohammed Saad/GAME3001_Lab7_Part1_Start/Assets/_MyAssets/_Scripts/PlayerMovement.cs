using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 20f; // Adjust this value to control player speed

    private Rigidbody2D rb;
    [SerializeField] GameObject torpedoPrefab;
    [SerializeField] float torpedoCooldown;
    [SerializeField] float torpedoLifeSpan;
    [SerializeField] Transform enemyTarget;
    private RangedCombatEnemy enemyHealth;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyHealth = GetComponent<RangedCombatEnemy>();
    }

    void Update()
    {
        // Get input from W, A, S, D keys
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Calculate movement direction
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);

        // Normalize the movement vector to avoid faster diagonal movement
        movement = movement.normalized * moveSpeed * Time.deltaTime;

        // Apply movement to the player's Rigidbody2D component
        rb.MovePosition(rb.position + movement);

        // Rotate the player based on movement direction
        if (moveHorizontal < 0)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 90f); // Rotate to face left
        }
        else if (moveHorizontal > 0)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, -90f); // Rotate to face right
        }

        if (moveVertical > 0)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f); // Rotate to face up
        }
        else if (moveVertical < 0)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 180f); // Rotate to face down
        }
        // Fire torpedo when spacebar is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FireTorpedo();
            enemyHealth.GetHealth();
            enemyHealth.ReduceHealth(25);
        }
    }
    private void FireTorpedo()
    {
        Game.Instance.SOMA.PlaySound("Torpedo");
        Game.Instance.SOMA.SetVolume(0.5f, SoundManager.SoundType.SOUND_SFX);

        Invoke("ReloadTorpedo", torpedoCooldown);
        GameObject torpedoInst = GameObject.Instantiate(torpedoPrefab, transform.position, Quaternion.identity);
        torpedoInst.GetComponent<EnemyTorpedoScript>().LockOnTarget(enemyTarget);
        Destroy(torpedoInst, torpedoLifeSpan);


    }
    private void ReloadTorpedo()
    {
        Debug.Log("Torpedo reloaded!");
        //readyToFire = true;
    }
}
