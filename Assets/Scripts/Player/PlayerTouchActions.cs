using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTouchActions : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] SwipeDetect swipeDetect;
    bool isMoving = false;
    public State currentState = State.Idle;

    Rigidbody2D rb;
    Animator animator;
    Touch touch;
    SwipeData data;

    Vector3 touchPosition;
    Vector3 whereToMove;

    float previousDistanceToTouchPos;
    float currentDistanceToTouchPos;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (currentState == State.Walk || currentState == State.Idle)
        {
            MovementUpdateDistance();
            CheckForTouch();
            MovementActionCheckEnd();
        }

        if (currentState != State.Attack && currentState != State.Stagger)
        {
            SwipeDetect.OnSwipe += SwipeDetect_OnSwipe;
            if(data.Direction != SwipeDirection.NotSet)
            {
                currentState = State.Attack;
                StartCoroutine(AttackCo());
            }
        }
    }

    private IEnumerator AttackCo()
    {
        MoveEnd();
        currentState = State.Attack;
        switch (data.Direction)
        {
            case SwipeDirection.Right:
                animator.SetFloat("moveX", 1);
                animator.SetFloat("moveY", 0);
                break;
            case SwipeDirection.Left:
                animator.SetFloat("moveX", -1);
                animator.SetFloat("moveY", 0);
                break;
            case SwipeDirection.Up:
                animator.SetFloat("moveX", 0);
                animator.SetFloat("moveY", 1);
                break;
            case SwipeDirection.Down:
                animator.SetFloat("moveX", 0);
                animator.SetFloat("moveY", -1);
                break;
        }
        animator.SetBool("attacking", true);
        yield return new WaitForSeconds(.01f);
        animator.SetBool("attacking", false);
        yield return new WaitForSeconds(.3f);
        currentState = State.Idle;
        data.Direction = SwipeDirection.NotSet;
    }

    //Check touch and set direction to walk
    public void CheckForTouch()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Ended)
            {
                previousDistanceToTouchPos = 0;
                currentDistanceToTouchPos = 0;
                isMoving = true;
                touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                touchPosition.z = 0;
                whereToMove = (touchPosition - transform.position).normalized;
                rb.velocity = new Vector2(whereToMove.x * moveSpeed, whereToMove.y * moveSpeed);
                PlayCorrectAnimation(whereToMove.x, whereToMove.y);
                currentState = State.Walk;
            }
        }
    }

    //Movement Check Functions
    private void MovementUpdateDistance()
    {
        if (isMoving)
        {
            currentDistanceToTouchPos = (touchPosition - transform.position).magnitude;
        }
    }
    
    private void MovementActionCheckEnd()
    { 
        if (currentDistanceToTouchPos > previousDistanceToTouchPos)
        {
            MoveEnd();
        }

        if (isMoving)
        {
            previousDistanceToTouchPos = (touchPosition - transform.position).magnitude;
        }
    }

    private void MoveEnd()
    {
        currentState = State.Idle;
        isMoving = false;
        rb.velocity = Vector2.zero;
        animator.SetBool("moving", false);
    }
    //End of Movement Check

    //Check for swipe
    private void SwipeDetect_OnSwipe(SwipeData obj)
    {
        data = obj;
    }

    //Determines which animation to pick
    public void PlayCorrectAnimation(float x, float y)
    {
        animator.SetBool("moving", true);

        if(Mathf.Abs(x) >= Mathf.Abs(y))
        {
            if(x > 0)
            {
                //Right
                animator.SetFloat("moveX", 1);
                animator.SetFloat("moveY", 0);
            }
            else
            {
                //Left
                animator.SetFloat("moveX", -1);
                animator.SetFloat("moveY", 0);
            }
        }
        else
        {
            if (y > 0)
            {
                //Up
                animator.SetFloat("moveX", 0);
                animator.SetFloat("moveY", 1);
            }
            else
            {
                //Down
                animator.SetFloat("moveX", 0);
                animator.SetFloat("moveY", -1);
            }
        }
    }

    //Knockback Calculation for player
    public void Knockback(float forceTime)
    {
        StartCoroutine(KnockbackCo(forceTime));
    }

    private IEnumerator KnockbackCo(float forceTime)
    {
        if(rb != null)
        {
            yield return new WaitForSeconds(forceTime);
            rb.velocity = Vector2.zero;
            currentState = State.Idle;
        }
    }
}
