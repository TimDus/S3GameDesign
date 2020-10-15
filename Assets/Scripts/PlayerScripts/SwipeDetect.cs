using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//internal info for swipes
public struct SwipeData
{
    public Vector2 StartPosition;
    public Vector2 EndPosition;
    public SwipeDirection Direction;
}

public enum SwipeDirection
{
    Up,
    Down,
    Left,
    Right,
    NotSet
}

public class SwipeDetect : MonoBehaviour
{
    private Vector2 fingerDownPosition;
    private Vector2 fingerReleasePosition;

    [SerializeField] private bool detectAfterRelease = false;
    [SerializeField] float minDistanceForSwipe = 20f;

    public static event Action<SwipeData> OnSwipe = delegate { };

    // Update is called once per frame
    void Update()
    {
        foreach(Touch touch in Input.touches)
        {
            if(touch.phase == TouchPhase.Began)
            {
                fingerDownPosition = touch.position;
                fingerReleasePosition = touch.position;
            }

            if(!detectAfterRelease && touch.phase == TouchPhase.Moved)
            {
                fingerDownPosition = touch.position;
                DetectSwipe();
            }

            if (touch.phase == TouchPhase.Ended)
            {
                fingerDownPosition = touch.position;
                DetectSwipe();
            }
        }
    }

    private void DetectSwipe()
    {
        if(SwipeDistanceCheck())
        {
            if(IsVerticalSwipe())
            {
                var direction = fingerDownPosition.y - fingerReleasePosition.y > 0 ? SwipeDirection.Up : SwipeDirection.Down;
                SendSwipe(direction);
            }
            else
            {
                var direction = fingerDownPosition.x - fingerReleasePosition.x > 0 ? SwipeDirection.Right : SwipeDirection.Left;
                SendSwipe(direction);
            }
        }
    }

    //Check Direction of swipe
    private bool IsVerticalSwipe()
    {
        return VerticalMovementDistance() > HorizontalMoveDistance();
    }

    //Send swipe data
    private void SendSwipe(SwipeDirection direction)
    {
        SwipeData swipeData = new SwipeData()
        {
            Direction = direction,
            StartPosition = fingerDownPosition,
            EndPosition = fingerReleasePosition
        };
        OnSwipe(swipeData);
    }

    //Swipe Distance Checking
    private bool SwipeDistanceCheck()
    {
        return VerticalMovementDistance() > minDistanceForSwipe || HorizontalMoveDistance() > minDistanceForSwipe;
    }

    private float HorizontalMoveDistance()
    {
        return Mathf.Abs(fingerDownPosition.x - fingerReleasePosition.x);
    }

    private float VerticalMovementDistance()
    {
        return Mathf.Abs(fingerDownPosition.y - fingerReleasePosition.y);
    }
}