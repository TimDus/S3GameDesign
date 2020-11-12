using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // Start is called before the first frame update
    private SwipeData data;
    public bool playerInRange;
    GameObject hit;

    // Start is called before the first frame update
    void Update()
    {
        SwipeDetect.OnSwipe += SwipeDetect_OnSwipe;
        if (data.Direction == SwipeDirection.Down && playerInRange)
        {
            GameObject ChildGameObject1 = hit.transform.GetChild(1).gameObject;
            data.Direction = SwipeDirection.NotSet;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        hit = other.gameObject;
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    private void SwipeDetect_OnSwipe(SwipeData obj)
    {
        data = obj;
    }
}
