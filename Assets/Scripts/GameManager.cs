using UnityEngine; 
using TMPro; 

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject meteorPrefab;
    [SerializeField] private TextMeshProUGUI scoreTMP;

    private readonly float screenBoundary = 8f;
    private float minMeteorDelay = 1f, maxMeteorDelay = 3f;  
    private int score = 0;

    // This is called a "Lambda" expression. It is very powerful, but here it just replaces a simple function body. This is the same as:
        // private void Start() 
        // {
        //     SpawnMeteor();
        // }
    private void Start() => SpawnMeteor();

    private void SpawnMeteor()
    {
        float xPosition = Random.Range(-screenBoundary, screenBoundary);
        float meteorDelay = Random.Range(minMeteorDelay, maxMeteorDelay);

        Vector3 meteorPosition = new(xPosition, transform.position.y, transform.position.z);
        Instantiate(meteorPrefab, meteorPosition, Quaternion.identity);

        // Using nameof() allows us to use the string name as a parameter without misspelling it
        Invoke(nameof(SpawnMeteor), meteorDelay);
    }

    public void AddPoint()
    {
        score++; // This increment operation is the same as the commented lines below:
        // score += 1;
        // score = score + 1;
        scoreTMP.text = $"Score: {score:D3}"; // This special line keeps the leading 000s in our score
    }
}
