using UnityEngine; 

[RequireComponent(typeof(Rigidbody2D))]

public class MeteorScript : MonoBehaviour
{
    [SerializeField] private float speed;

    private GameManager gameManager;
    private Rigidbody2D rigidBody;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        gameManager = FindFirstObjectByType<GameManager>();
    }

    private void Start() => rigidBody.linearVelocityY = speed; 
    // This is called a "Lambda" expression. It is very powerful, but here it just replaces a simple function body. This is the same as:
        // private void Start() 
        // {
        //     rigidBody.linearVelocityY = speed;
        // }

    // Lambda syntax doesn't work with complex logic like if statements or with multiple lines
    private void Update()
    {
        // Simple if statements with 1 line can be written without the {} body syntax
        if (transform.position.y <= -6f) Destroy(gameObject); 
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Mothership")) Debug.Log("Meteor collided with Mothership!"); 

        else if (other.CompareTag("Projectile")) // Don't forget to make this tag and assign it in the Inspector
        {
            Destroy(other.gameObject);  
            gameManager.AddPoint(); 
        }

        Destroy(gameObject); 
    }
}
