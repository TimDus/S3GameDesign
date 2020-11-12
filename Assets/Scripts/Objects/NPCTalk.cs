using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCTalk : MonoBehaviour
{
    private SwipeData data;
    public GameObject dialogBox;
    public Text dialogText;
    public string dialog;
    public bool playerInRange;

    // Start is called before the first frame update
    void Update()
    {
        SwipeDetect.OnSwipe += SwipeDetect_OnSwipe;
        if (data.Direction == SwipeDirection.Down && playerInRange)
        {
            if (dialogBox.activeInHierarchy)
            {
                dialogBox.SetActive(false);
            }
            else
            {
                dialogBox.SetActive(true);
                dialogText.text = dialog;
            }
            data.Direction = SwipeDirection.NotSet;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
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
            dialogBox.SetActive(false);
        }
    }

    private void SwipeDetect_OnSwipe(SwipeData obj)
    {
        data = obj;
    }
}
