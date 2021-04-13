using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class PathController : MonoBehaviour
{
    [SerializeField] List<Transform> waypointsObjects;

    private Vector3[] waypoints;
    private Tween pathTween;
    private const int PATH_TIME = 30;

    // Start is called before the first frame update
    void Start()
    {
        EventBroker.GameOver += StopPathTween;

        waypoints = new Vector3[waypointsObjects.Count];

        for (int i = 0; i < waypointsObjects.Count; i++)
        {
            waypoints[i] = waypointsObjects[i].transform.position;
        }

        pathTween = gameObject.transform.DOPath(waypoints, PATH_TIME, PathType.Linear, PathMode.Full3D, 10, Color.red).SetEase(Ease.Linear).SetLoops(-1);
    }

    private void StopPathTween()
    {
        pathTween.Kill();
    }
}
