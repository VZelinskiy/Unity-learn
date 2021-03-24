using UnityEngine;
using UnityEngine.UI;

public class ThrowBall : MonoBehaviour
{
    [SerializeField] GameObject ball;
    [SerializeField] GameObject ballPlaceholder;
    [SerializeField] float throwForce;
    [SerializeField] Vector3 verticalOffset;
    [SerializeField] Slider throwSlider;

    private Rigidbody ballRb;
    private bool isThrowed;
    private float minSpawnX = 3;
    private float maxSpawnX = 7.5f;
    private float rangeSpawnZ = 4;
    private float throwTimer = 0;
    private float maxThrowTime = 1;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        ballRb = ball.GetComponent<Rigidbody>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            throwTimer += Time.deltaTime;
            throwSlider.value = throwTimer;
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyUp(KeyCode.Space) && !isThrowed && gameManager.isGameActive)
        {
            if (throwTimer > maxThrowTime)
            {
                throwTimer = maxThrowTime;
            }
            Throw();
            throwTimer = 0;
        }
    }

    public void RespawnPosition()
    {
        ballRb.velocity = Vector3.zero;
        ballPlaceholder.SetActive(true);
        ball.SetActive(false);
        isThrowed = false;
        transform.position = new Vector3(Random.Range(minSpawnX, maxSpawnX), transform.position.y, Random.Range(-rangeSpawnZ, rangeSpawnZ));
    }

    private void Throw()
    {
        ball.transform.position = ballPlaceholder.transform.position;
        ball.SetActive(true);
        ballPlaceholder.SetActive(false);
        ballRb.AddForce((ball.transform.position - transform.position + verticalOffset).normalized * throwForce * throwTimer, ForceMode.Impulse);
        throwSlider.value = 0;
        isThrowed = true;
    }
}
