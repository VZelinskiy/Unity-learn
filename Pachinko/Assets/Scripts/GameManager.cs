using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public GameObject BouncingBall;
    public GameObject BowlingBall;
    public Text ScoreBowling;
    public Text ScoreBouncing;

    private int BowlingBallsCount;
    private int BouncingBallsCount;

    // Start is called before the first frame update
    void Start()
    {
        ScoreBowling.text = "0";
        ScoreBouncing.text = "0";

        BowlingBallsCount = 0;
        BouncingBallsCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        LaunchBall();
    }

    public void UpdateBowlingScore()
    {
        BowlingBallsCount += 1;
        ScoreBowling.text = BowlingBallsCount.ToString();
    }

    public void UpdateBouncingScore()
    {
        BouncingBallsCount += 1;
        ScoreBouncing.text = BouncingBallsCount.ToString();
    }

    private void LaunchBall()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Vector2 bouncingBallPosition = new Vector2(Random.Range(-8, 8), 4.2f);
            Instantiate(BouncingBall, bouncingBallPosition, Quaternion.identity);
        }

        if (Input.GetButtonDown("Fire2"))
        {
            Vector2 bowlingBallPosition = new Vector2(Random.Range(-8, 8), 4.2f);
            Instantiate(BowlingBall, bowlingBallPosition, Quaternion.identity);
        }
    }
}
