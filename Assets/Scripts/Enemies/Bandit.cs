using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bandit : Enemy
{
    private Rigidbody2D myRigidbody;
    public Transform homePosition;
    public Transform target;
    public float chaseRadius;
    public float attackRadius;
    bool attacked;
    bool cooldownStart;
    Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        cooldownStart = false;
        attacked = false;
        myRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        target = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(!attacked)
        {
            cooldownStart = false;
            if (currentState != State.Stagger || currentState != State.Attack)
            {
                CheckDistance();
            }
            if (Vector3.Distance(target.position, transform.position) <= attackRadius && currentState != State.Attack)
            {
                currentState = State.Attack;
                StartCoroutine(Attack(target.position.x, target.position.y));
            }
        }
        if(attacked && !cooldownStart)
        {
            cooldownStart = true;
            StartCoroutine(Countdown());
        }
    }

    void CheckDistance()
    {
        if (Vector3.Distance(target.position, transform.position) <= chaseRadius && Vector3.Distance(target.position, transform.position) > attackRadius)
        {
            if (currentState == State.Idle || currentState == State.Walk && currentState != State.Stagger)
            {
                ChangeState(State.Walk);
                Vector3 temp = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
                changeAnimation(temp - transform.position);
                myRigidbody.MovePosition(temp);
            }
        }
        else
        {
            MoveEnd();
        }
    }

    public void SetAnimationFloat(Vector2 setVector)
    {
        animator.SetFloat("moveX", setVector.x);
        animator.SetFloat("moveY", setVector.y);
    }

    public void changeAnimation(Vector2 direction)
    {
        animator.SetBool("moving", true);
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x > 0)
            {
                SetAnimationFloat(Vector2.right);
            }
            else if (direction.x < 0)
            {
                SetAnimationFloat(Vector2.left);
            }
        }
        else if (Mathf.Abs(direction.x) < Mathf.Abs(direction.y))
        {
            if (direction.y > 0)
            {
                SetAnimationFloat(Vector2.up);
            }
            else if (direction.y < 0)
            {
                SetAnimationFloat(Vector2.down);
            }
        }
    }

    private IEnumerator Attack(float x, float y)
    {
        MoveEnd();
        animator.SetBool("attacking", true);
        currentState = State.Attack;
        yield return new WaitForSeconds(.01f);
        animator.SetBool("attacking", false);
        yield return new WaitForSeconds(1f);
        currentState = State.Idle;
        MoveEnd();
        attacked = true;
    }

    private IEnumerator Countdown()
    {
        yield return new WaitForSeconds(1.5f);
        attacked = false;
    }

    private void MoveEnd()
    {
        animator.SetBool("moving", false);
    }

    private void ChangeState(State newState)
    {
        if (currentState != newState)
        {
            currentState = newState;
        }
    }
}