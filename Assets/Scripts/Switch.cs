using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    public GameObject Laser;

    [SerializeField] private Color inactiveColor;
    [SerializeField] private Color activeColor;

    private MeshRenderer switchMeshRenderer;

    private bool isOn;

    [SerializeField] private float pressDelay = 2f;
    private float timer = 0;

    void Start()
    {
        switchMeshRenderer = GetComponentInChildren<MeshRenderer>();

        isOn = true;
        FlipSwitch(isOn, activeColor);
    }

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (timer < 0)
        {
            if (col.gameObject.tag == "Player" && isOn)
            {
                isOn = false;
                FlipSwitch(isOn, inactiveColor);
            }
            else if (col.gameObject.tag == "Player" && !isOn)
            {
                isOn = true;

                FlipSwitch(isOn, activeColor);
            }
        }
    }

    private void FlipSwitch(bool isOn, Color color)
    {
        //Collision with palyer makes the switch to inactive color
        gameObject.GetComponent<MeshRenderer>().material.color = color;

        //Disabling the scripts after colliding with the player
        Laser.GetComponent<Laser>().enabled = isOn;
        Laser.GetComponent<LineRenderer>().enabled = isOn;

        timer = pressDelay;
    }
}
