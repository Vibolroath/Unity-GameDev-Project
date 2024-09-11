using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformHorizontal : MonoBehaviour
{
    [SerializeField] private Transform pathStart;
    [SerializeField] private Transform pathEnd;
    [SerializeField] private float speed;  // speed (m/s) of the platform's movement

    private Transform platform;  // the platform to be moved

    enum AxisState
    {
        XAxis,
        ZAxis,
        Both
    }
    [SerializeField] private AxisState axisState;


    private void Start()
    {
        platform = transform.GetChild(0);

        StartCoroutine(MovePlatform(pathStart.position));
    }


    private void Update()
    {
        Vector3 startPos = pathStart.position;
        Vector3 endPos = pathEnd.position;
        switch (axisState)
        {
            case (AxisState.XAxis):
                PathCheck(platform.position.x, startPos.x, endPos.x);
                break;

            case (AxisState.ZAxis):
                PathCheck(platform.position.z, startPos.z, endPos.z);
                break;

            case (AxisState.Both):
                PathCheck(platform.position, startPos, endPos);
                break;
        }
    }
    private void PathCheck(float positionAxis, float startAxis, float endAxis)
    {
        if (positionAxis == startAxis)
        {
            StartCoroutine(MovePlatform(pathEnd.position));
        }
        else if (positionAxis == endAxis)
        {
            StartCoroutine(MovePlatform(pathStart.position));
        }
    }

    private void PathCheck(Vector3 position, Vector3 startPosition, Vector3 endPosition) // determines which point the platform will move to.
    {
        if (position.x == startPosition.x
            && position.z == startPosition.z)
        {
            StartCoroutine(MovePlatform(pathEnd.position));
        }
        else if (position.x == endPosition.x
            && position.z == endPosition.z)
        {
            StartCoroutine(MovePlatform(pathStart.position));
        }

    }

    private IEnumerator MovePlatform(Vector3 target) //Coroutine: performs different path movements depending on designer-chosen state.
    {
        Vector3 position = platform.position;
        float time = 0f;
        float x;
        float z;

        switch (axisState)
        {
            case (AxisState.XAxis):
                while (platform.position.x != target.x)
                {
                    x = Mathf.Lerp(position.z, target.z,
                    (time / Vector3.Distance(position, target)) * speed);
                    platform.position =
                        new Vector3(Mathf.Lerp(position.x, target.x,
                        (time / Vector3.Distance(position, target)) * speed),
                        platform.position.y, platform.position.z);
                    time += Time.deltaTime;
                    yield return null;
                }
                break;

            case (AxisState.ZAxis):
                while (platform.position.z != target.z)
                {
                    z = Mathf.Lerp(position.z, target.z,
                    (time / Vector3.Distance(position, target)) * speed);
                    platform.position = new Vector3(position.x, position.y, z);
                    time += Time.deltaTime;
                    yield return null;
                }
                break;

            case (AxisState.Both):
                while (platform.position.x != target.x && platform.position.z != target.z)
                {
                    x = Mathf.Lerp(position.x, target.x,
                        (time / Vector3.Distance(position, target)) * speed);
                    z = Mathf.Lerp(position.z, target.z,
                    (time / Vector3.Distance(position, target)) * speed);

                    platform.position = new Vector3(x, position.y, z);
                    time += Time.deltaTime;
                    yield return null;
                }
                break;
        }
    }
}
