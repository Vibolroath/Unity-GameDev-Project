using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private LineRenderer lr;

    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;

    RaycastHit hit;

    void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    void Update()
    {
        //Set the positions of the line renderer
        lr.SetPosition(0, startPoint.position);
        lr.SetPosition(1, endPoint.position);

        //Raycast function to kill the player when collide with the laser
        if (Physics.Raycast(startPoint.position, -transform.forward, out hit))
        {
            if (hit.transform.CompareTag("Player"))
            {
                Kill(GameManager.Instance.Player);
            }
        }
    }

    //Die function
    private void Kill(Player player)
    {
        player.Die();
    }


}
