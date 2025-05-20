using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    public float moveSpeed = 20;

    private Rigidbody2D rb;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = gameObject;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    {
        Vector2 movement = Vector2.zero;

        if (gameObject.name == "Player")
        {
            if (Input.GetKey(KeyCode.W))
            {
                movement.y = 1f;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                movement.y = -1f;
            }

            if (Input.GetKey(KeyCode.A))
            {
                movement.x = -1f;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                movement.x = 1f;
            }
        }

        movement = movement.normalized;

        rb.AddForce(movement * moveSpeed, ForceMode2D.Force);
    }
}