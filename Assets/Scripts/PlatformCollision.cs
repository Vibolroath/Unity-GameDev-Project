using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        GameObject target = collision.gameObject;
        if (target.transform.position.y > transform.position.y)
        target.transform.parent = transform;
    }
    private void OnCollisionExit(Collision collision)
    {
        collision.gameObject.transform.parent = null;
    }
}
