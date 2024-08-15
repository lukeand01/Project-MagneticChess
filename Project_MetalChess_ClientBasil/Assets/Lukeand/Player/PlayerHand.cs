using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    Magnetic currentMagnetTarget;
    bool grabbedMagnet;
    bool orderForPlacingMagnet;

    Vector3 idlePosition;
    Vector3 targetPosition; //where it should be going. however if its going 
    Vector3 storedPosition; //any position passed 

    bool shouldWaitTillItGetsTothePos; //this locks the thing from moving.
    bool isMoving;
    bool hasSomethingToDo;

    private void Awake()
    {
        idlePosition = transform.position;
        targetPosition = transform.position;
    }

    //only when it completes grab it, can it take to the board
    //cancel can always be called.

    private void Update()
    {
        if (!isMoving) return;

        float distance = Vector3.Distance(transform.position, targetPosition);
        if (transform.position != targetPosition)
        {
            //move towards it
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * 15);

            if(currentMagnetTarget != null && grabbedMagnet)
            {
                currentMagnetTarget.transform.position = transform.position + new Vector3(0, -0.2f, 0);
            }
            
            if(currentMagnetTarget != null)
            {
                currentMagnetTarget.ControlCollider(false);
            }

        }
        else if(hasSomethingToDo)
        {
           //either grab the magnetic or place the magnetic


            if (grabbedMagnet)
            {
                //this means we are placing it. so we put the magnetic at the position
                currentMagnetTarget.transform.position = targetPosition;
                currentMagnetTarget.SetTable();
                currentMagnetTarget.ControlCollider(true);
                BecomeIdle();
            }
            else
            {
                //this means we are moving to get it.
                grabbedMagnet = true;

                if (orderForPlacingMagnet)
                {
                    orderForPlacingMagnet = false;
                    PlaceIt(storedPosition);
                    return;
                }

            }


            hasSomethingToDo = false;
        }



    }

    public void GrabIt(Magnetic magnetic)
    {
        //we give to the closet one.
        isMoving = true;
        currentMagnetTarget = magnetic;

        targetPosition = currentMagnetTarget.transform.position + new Vector3(0,0.2f,0);
        hasSomethingToDo = true;


        //now this thing has to move towards it.
    }
    public void PlaceIt(Vector3 positionToPlace)
    {
        //and the magnet should be taken wtih the hand.

        if(currentMagnetTarget == null) return;

        if (!grabbedMagnet)
        {
            //we are going to put an order to take it
            storedPosition = positionToPlace;
            orderForPlacingMagnet = true;
            return;
        }

        targetPosition = positionToPlace;
        hasSomethingToDo = true;
    }
    public void BecomeIdle()
    {
        targetPosition = idlePosition;
        currentMagnetTarget = null;
        hasSomethingToDo = false;
        grabbedMagnet = false;
    }


    //to become idle.
    //

}
