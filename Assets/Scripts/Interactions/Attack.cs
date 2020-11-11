using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] private float force = 2f;
    [SerializeField] private float forceTime = 0.2f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Enemy") || other.CompareTag("Player"))
        {
            Rigidbody2D hit = other.GetComponent<Rigidbody2D>();
            if(hit != null)
            {
                Vector2 difference = hit.transform.position - transform.position;
                difference = difference.normalized * force;
                hit.AddForce(difference, ForceMode2D.Impulse);
                if (other.gameObject.CompareTag("Enemy"))
                {
                    hit.isKinematic = false;
                    hit.GetComponent<Enemy>().currentState = State.Stagger;
                    other.GetComponent<Enemy>().Knockback(hit, forceTime);

                }

                if(other.GetComponentInParent<PlayerTouchActions>() != null)
                {
                    if (other.GetComponentInParent<PlayerTouchActions>().currentState != State.Stagger)
                    {
                        hit.GetComponent<PlayerTouchActions>().currentState = State.Stagger;
                        other.GetComponent<PlayerTouchActions>().Knockback(forceTime);
                    }
                }
            }
        }
    }
}
