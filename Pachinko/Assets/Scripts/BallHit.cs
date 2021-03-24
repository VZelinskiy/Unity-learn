using UnityEngine;

public class BallHit : MonoBehaviour
{
    public string BowlingBallTag;
    public string BouncingBallTag;
    public GameManager GamaManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(BowlingBallTag))
        {
            GamaManager.UpdateBowlingScore();
        }

        if (collision.gameObject.CompareTag(BouncingBallTag))
        {
            GamaManager.UpdateBouncingScore();
        }
        
        Destroy(collision.gameObject);
    }
}
