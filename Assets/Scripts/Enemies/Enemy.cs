using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public State currentState;
    public int health;
    public string enemyName;
    public int baseAttack;
    public float moveSpeed;

    public void Knockback(Rigidbody2D myRigidbody, float forceTime)
    {
        StartCoroutine(KnockbackCo(myRigidbody, forceTime));
    }

    private IEnumerator KnockbackCo(Rigidbody2D myRigidbody, float forceTime)
    {
        if (myRigidbody != null)
        {
            yield return new WaitForSeconds(forceTime);
            myRigidbody.velocity = Vector2.zero;
            currentState = State.Idle;
            GetComponent<Rigidbody2D>().isKinematic = true;
        }
    }
}
