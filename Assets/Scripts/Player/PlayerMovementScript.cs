using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    public float acceleration = 50f;
    public float turnSpeed = 200f;
    public float maxSpeed = 50f;
    public float baseSpeed = 50f;
    public float sprintSpeed = 100f;
    public float drag = 0.95f;

    private float stationaryStartRotation = 0f;
    private bool isStationary = false;

    private Rigidbody2D rigidBody;
    private float moveInput;
    private float turnInput;

    public float recordSpacing = 0.1f;
    private List<Vector2> positions = new List<Vector2>();

    private Vector2 lastRecordedPosition;

    private List<Vector2> positionsHistory = new List<Vector2>();
    public int maxHistory = 500;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        lastRecordedPosition = transform.position;
        positions.Add(lastRecordedPosition);
    }

    void FixedUpdate()
    {
        Movement();

        positionsHistory.Insert(0, rigidBody.position);

        if (positionsHistory.Count > maxHistory)
            positionsHistory.RemoveAt(positionsHistory.Count - 1);

        if (Vector2.Distance(lastRecordedPosition, (Vector2)transform.position) > recordSpacing)
        {
            lastRecordedPosition = transform.position;
            positions.Insert(0, lastRecordedPosition);
        }
    }

    public Vector2 GetPositionAt(int index)
    {
        if (index < positions.Count)
            return positions[index];
        else
            return positions[positions.Count - 1];
    }
    public int PositionsCount => positions.Count;

    void Movement()
    {
        
        // Handle sprinting
        float targetMaxSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : baseSpeed;

        // Handle input
        moveInput = Input.GetKey(KeyCode.W) ? 1f : 0f;
        turnInput = 0f;

        // Determine target forward speed
        float currentForwardSpeed = Vector2.Dot(rigidBody.velocity, transform.up);
        float targetSpeed = moveInput * targetMaxSpeed;

        // Choose acceleration or deceleration based on grounded or air (optional)
        float forwardAcceleration = acceleration;

        // Use Mathf.MoveTowards to control velocity
        float newForwardSpeed = Mathf.MoveTowards(currentForwardSpeed, targetSpeed, forwardAcceleration * Time.fixedDeltaTime);
        
        if (moveInput > 0)
        {
            rigidBody.drag = 10;
        }
        else
        {
            rigidBody.drag = 100;
        }
        
        // Update rigidbody velocity in the forward direction only
        Vector2 forwardVelocity = transform.up * newForwardSpeed;

        // Preserve lateral (sideways) velocity (optional if you want drifting)
        Vector2 sideways = Vector2.Dot(rigidBody.velocity, transform.right) * transform.right;
        rigidBody.velocity = forwardVelocity + sideways;

        // Handle rotation
        if (rigidBody.velocity.magnitude > 0.1f)
        {
            isStationary = false;

            if (Vector2.Dot(rigidBody.velocity.normalized, transform.up) > 0.1f)
            {
                if (Input.GetKey(KeyCode.A)) turnInput = 1f;
                else if (Input.GetKey(KeyCode.D)) turnInput = -1f;
            }
            else
            {
                if (Input.GetKey(KeyCode.A)) turnInput = -1f;
                else if (Input.GetKey(KeyCode.D)) turnInput = 1f;
            }
        }
        else
        {
            if (!isStationary)
            {
                stationaryStartRotation = rigidBody.rotation;
                isStationary = true;
            }

            if (Input.GetKey(KeyCode.A)) turnInput = 1f;
            else if (Input.GetKey(KeyCode.D)) turnInput = -1f;
        }

        // Apply rotation
        float newRotation = rigidBody.rotation;
        if (turnInput != 0)
        {
            float turnAmount = turnInput * turnSpeed * Time.fixedDeltaTime;
            newRotation += turnAmount;

            if (isStationary)
            {
                newRotation = Mathf.Clamp(newRotation, stationaryStartRotation - 90f, stationaryStartRotation + 90f);
            }

            rigidBody.MoveRotation(newRotation);
        }
        else if (isStationary)
        {
            float centeredRotation = Mathf.LerpAngle(rigidBody.rotation, stationaryStartRotation, Time.fixedDeltaTime * 2f);
            rigidBody.MoveRotation(centeredRotation);
        }
    }

    public Vector2 GetPositionAtDelay(int delay)
    {
        if (delay < positionsHistory.Count)
            return positionsHistory[delay];
        else
            return rigidBody.position;
    }

    public float CurrentSpeed()
    {
        return rigidBody.velocity.magnitude;
    }

    public Vector2 GetCurrentPosition()
    {
        return rigidBody.position;
    }

    public Vector2 GetForwardDirection()
    {
        return transform.up;
    }
}
