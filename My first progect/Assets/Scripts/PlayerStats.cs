using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] AudioClip crushSound;
    [SerializeField] AudioClip biteSound;
    [SerializeField] AudioClip foodPickupSound;
    [SerializeField] AudioClip powerupPickupSound;
    [SerializeField] ParticleSystem bloodParticle;
    [SerializeField] ParticleSystem crushParticle;

    private AudioSource playerAudio;
    private int lives;
    private int totalPoints;
    private readonly int foodValue = 1;
    private readonly int maxLives = 3;

    // Start is called before the first frame update
    void Start()
    {
        playerAudio = GetComponent<AudioSource>();

        lives = maxLives;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            playerAudio.PlayOneShot(biteSound);
            bloodParticle.Play();
            TakeHit();
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            playerAudio.PlayOneShot(crushSound);
            crushParticle.Play();
            TakeHit();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Powerup"))
        {
            playerAudio.PlayOneShot(powerupPickupSound);
            ObjectPooler.SharedInstance.SetObjectToPool(other.gameObject);
            HeartCollected();
        }
        else if (other.gameObject.CompareTag("Food"))
        {
            playerAudio.PlayOneShot(foodPickupSound);
            ObjectPooler.SharedInstance.SetObjectToPool(other.gameObject);
            UpdateScore();
        }
    }

    private void TakeHit()
    {
        lives--;
        EventBroker.CallLifeLost(lives);

        if (lives == 0)
        {
            StartCoroutine(GameManager.Instance.GameOver());
        }
    }

    private void HeartCollected()
    {
        if (lives < maxLives)
        {
            EventBroker.CallLifeGain(lives);
            lives++;
        }
    }

    private void UpdateScore()
    {
        totalPoints += foodValue;
        EventBroker.CallUpdateScoreForFood(totalPoints);
    }
}
