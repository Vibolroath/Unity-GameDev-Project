using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    enum State
    {
        Walking,
        Jumping,
    };

    private State state;

    public Rigidbody m_rigidBody;

    private float dx;
    private float dy;

    bool canJump;

    Vector3 moveDirection;

    private float currentSpeed;
    [SerializeField] private float maxSpeed = 30.0f;
    [SerializeField] private float defaultSpeed = 20.0f;
    private float forceToAdd;

    [SerializeField] private float jumpForce = 7.0f;
    [SerializeField] private float trampForce = 25.0f;

    [SerializeField] private float groundDistance = 0.5f;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask trampLayer;

    private bool isInTeleporter = false;

    void Start()
    {
        m_rigidBody = GetComponent<Rigidbody>();
        m_rigidBody.isKinematic = false;

        state = State.Walking;
    }

    void FixedUpdate()
    {

        switch (state)
        {
            case State.Walking:

                if (canJump)
                {
                    state = State.Jumping;
                }
                else if (moveDirection == Vector3.zero)
                {
                    return;
                }

                //Looking and rotating towards the direction that the player is moving in
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                targetRotation = Quaternion.RotateTowards(
                    transform.rotation,
                    targetRotation,
                    360 * Time.fixedDeltaTime);

                //Add force for walking in accelerating speed and rotating the player to target direction
                m_rigidBody.AddForce(forceToAdd * moveDirection);
                m_rigidBody.MoveRotation(targetRotation);

                if (IsOnGround())
                {
                    m_rigidBody.drag = 0.0f;
                }
                break;

            case State.Jumping:
                if (canJump)
                {
                    //Setting the velocity to zero so there is no added velocity when jumping
                    m_rigidBody.velocity = Vector3.zero;
                    //Add force to jump in upward direction
                    m_rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                }
                else if (!IsOnGround())
                {
                    state = State.Walking;
                    m_rigidBody.drag = 1.5f;
                }
                break;
        }
    }

    void Update()
    {
        //get the input axes and stting directions
        dx = Input.GetAxis(InputAxes.Horizontal);
        dy = Input.GetAxis(InputAxes.Vertical);
        moveDirection = new Vector3(dx, 0, dy).normalized;

        //setting the maximum acceleration speed
        currentSpeed = m_rigidBody.velocity.magnitude;
        forceToAdd = defaultSpeed * (1 - currentSpeed / maxSpeed);

        OnSpacePressed(); //checks which function to perform (scene change OR jump)
    }

    //checks if the player is on/off the ground using raycast
    private bool IsOnGround()
    {
        return Physics.Raycast(transform.position, Vector3.down, groundDistance, groundLayer);
    }
    
    //checks for player collisions with trampolines/moving platforms
    void OnCollisionEnter(Collision col)
    {
        GameObject other = col.gameObject;
        if(trampLayer.Contains(other) && !IsOnGround())
        {
            m_rigidBody.velocity = Vector3.zero;
            m_rigidBody.AddForce(Vector3.up * trampForce, ForceMode.Impulse);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Checkpoint"))
        {
            other.gameObject.GetComponent<Checkpoint>().UpdateCheckpoint();
            return;
        }
        
        if (other.gameObject .CompareTag("Teleporter"))
        {
            isInTeleporter = true;
        }

    }

private void OnTriggerStay(Collider other)
    {
    if (other.CompareTag("Teleporter"))
        {
            OnSpacePressed(); // go to next scene in build list if in Teleporter
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isInTeleporter = false;
    }
    public void Die()
    {
        gameObject.SetActive(false);
        GameManager.Instance.Die();
    }

    public void Revive()
    {
        gameObject.SetActive(true);
    }

    private void OnSpacePressed() //Determines whether the player will teleport or jump
    {
            if (isInTeleporter == true && Input.GetButtonDown("Jump"))  // if player is inside the teleport
            {
                GameManager.Instance.NextLevel();
            }
            else
            {
                Jump();
            }
    }

    private void Jump() //Controls whether Player can Jump or not
    {
        if (Input.GetButtonDown("Jump") && IsOnGround())
        {
            canJump = true;
        }
        else if (Input.GetButtonUp("Jump") || !IsOnGround())
        {
            canJump = false;
        }
    }
}
