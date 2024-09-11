using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformVertical : MonoBehaviour
{
    [SerializeField] private Transform pathStart;
    [SerializeField] private Transform pathEnd;

    [SerializeField] private float speed;  // speed (m/s) of the platform's movement

    private Transform platform;  // the platform to be moved

    void Start()
    {
        platform = transform.GetChild(0);

        StartCoroutine(MovePlatform(pathStart.position));
    }

    void Update()
    {
        if (platform.position.y == pathEnd.position.y)
        {
            StartCoroutine(MovePlatform(pathStart.position));
        }
        else if (platform.position.y == pathStart.position.y)
        {
            StartCoroutine(MovePlatform(pathEnd.position));
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.parent = gameObject.transform;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.parent = null;
        }
    }

    private IEnumerator MovePlatform(Vector3 targetLocation)   /// Coroutine: moves platform to given position
    {
        Vector3 position = platform.position;
        float time = 0f;
        while (platform.position.y != targetLocation.y)
        {
            float y = Mathf.Lerp(position.y, targetLocation.y,
                (time / Vector3.Distance(position, targetLocation)) * speed);
            platform.position = new Vector3(platform.position.x, y, platform.position.z);
            time += Time.deltaTime;
            yield return null;
        }
    }
}
