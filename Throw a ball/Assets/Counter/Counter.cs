using UnityEngine;
using UnityEngine.UI;

public class Counter : MonoBehaviour
{
    public Text CounterText;

    private ThrowBall throwBall;
    private int Count = 0;

    private void Start()
    {
        Count = 0;
        throwBall = GameObject.Find("Player").GetComponent<ThrowBall>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Box"))
        {
            Count += 1;
            CounterText.text = "Count : " + Count;
        }
        throwBall.RespawnPosition();
    }
}
