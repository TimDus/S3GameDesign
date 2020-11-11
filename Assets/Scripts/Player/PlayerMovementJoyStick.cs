using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementJoyStick : MonoBehaviour
{
    public Joystick joystick;
    public float speed = 40f;
    private Rigidbody2D rb;
    private Vector3 movement;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        movement = Vector3.zero;
        movement.x = joystick.Horizontal;
        movement.y = joystick.Vertical;
        float storemove = movement.x;
        if (movement != Vector3.zero)
        {
            MoveCharacter();
            if (movement.x >= 0)
            {
                if (movement.y <= 0.5)
                {
                    movement.x = 1;
                }
            }
            else
            {
                if (movement.y <= 0.5)
                {
                    movement.x = -1;
                }
            }
            if (movement.y >= 0)
            {
                movement.y = 1;
            }
            else
            {
                movement.y = -1;
            }
            animator.SetFloat("moveX", movement.x);
            if (storemove <= 0.5 || storemove >= -0.5)
            {
                animator.SetFloat("moveY", movement.y);
            }
            else
            {
                animator.SetFloat("moveY",0);
            }
        }
    }

    void MoveCharacter()
    {
        rb.MovePosition(transform.position + movement * speed * Time.deltaTime);
    }
}
