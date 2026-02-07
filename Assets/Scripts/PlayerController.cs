using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]        // Since we know we will always need a RigidBody2D and a PolygonCollider2D on this gameobject, 
[RequireComponent(typeof(PolygonCollider2D))]  // we can use the [RequireComponent] attribute so that Unity knows to always add them for us
public class PlayerController : MonoBehaviour 
{
    [SerializeField] private GameObject projectilePrefab; 
    [SerializeField] private float speed; 

    #region Respawning Variables
    [Header("Respawning")] // The [Header] attribute adds an organizational header in the Inspector
    [SerializeField] private float respawnSpeed; // Change how fast the Player respawns
    [SerializeField] private Vector3 spawnStart, spawnEnd; // When respawning, the Player starts at spawnStart and moves to spawnEnd
    #endregion

    private Rigidbody2D body;
    private PolygonCollider2D polygonCollider;

    private readonly float screenBoundary = 8f; // It's best practice to declare variables as readonly when you don't need to change their values

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        polygonCollider = GetComponent<PolygonCollider2D>();
    }

    #region Mothership Collision
    // In this example, Mothership's PolygonCollider2D.isTrigger is True. Make sure you use OnCollisionEnter2D if this is not the case for you.
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Mothership")) // Don't forget to make this tag and assign it in the Inspector!
        {
            // Since the Meteors push the Player in this version, we fix the velocity to stop the Player falling after respawning
            body.linearVelocity = Vector2.zero;
            
            polygonCollider.isTrigger = true; // The collider is still enabled, but the Meteors won't push it while respawning
            gameObject.tag = "Respawn"; // Set the tag to Respawn so our Update() logic knows the Player is respawning
            
            transform.position = spawnStart; // Teleport the Player to spawnStart
        }
    }
    #endregion

    void Update()
    {
        #region Respawning Logic
        // We fix the Player's velocity in OnTriggerEnter2D, but the Meteors also rotate it, so we fix that in Update()
        transform.rotation = new Quaternion(0f, 0f, 180f, 1f);

        // Before checking for input, we check to see if the Player is respawning
        if (gameObject.CompareTag("Respawn"))
        {
            // If it is, and it is still below spawnEnd, we move it at spawnSpeed towards spawnEnd
            if (transform.position.y < spawnEnd.y) transform.position = Vector3.MoveTowards(
                transform.position,
                spawnEnd,
                respawnSpeed * Time.deltaTime
            );

            // If Player already caught up to spawnEnd, we re-enable physics interactions with Meteors and change the tag back
            else
            {
                gameObject.tag = "Player";
                polygonCollider.isTrigger = false;
            }
        }
        #endregion
        
        else // (If Player isn't respawning)
        {
            float xInput = Input.GetAxis("Horizontal");
            transform.Translate(xInput * speed * Time.deltaTime, 0f, 0f);
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, -screenBoundary, screenBoundary), transform.position.y, transform.position.z);

            if (Input.GetButtonDown("Jump"))
            {
                // Since we already specified the type as Vector3, the new() keyword doesn't need to know it again. The two lines below are equivalent.
                    // Vector3 position = new(float, float, float);
                    // Vector3 position = new Vector3(float, float, float);
                Vector3 position = new(transform.position.x, transform.position.y + 1.2f, transform.position.z);
                Instantiate(projectilePrefab, position, Quaternion.identity);
            }
        }
    }
    
}
