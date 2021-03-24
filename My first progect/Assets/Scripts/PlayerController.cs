using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private readonly float speed = 30;   
    private Rigidbody playerRb;
    
    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MovePlayer();
    }

    //Moves the player based on arrow key input
    void MovePlayer()
    {
        float verticalInput = Input.GetAxis("Vertical");
        playerRb.AddForce(Vector3.up * speed * verticalInput);
    }
}
