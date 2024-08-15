using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //this will have hands
    //this will have surfaces.
    //this will have a camera as well.

    [Separator("SURFACES")]
    [SerializeField] Renderer player1_Surface; //this is teh place where we will get 


    public List<Vector3> GetPositionsToAllignMagnetPieces( int pieceQuantity)
    {
        List<Vector3> posList = new();
        Renderer playerSurface = player1_Surface;


        float surfaceLenght = playerSurface.bounds.size.x;
        float surfaceStartPoint = playerSurface.bounds.min.x;

        float segmentLenght = surfaceLenght / (pieceQuantity - 1);


        for (int i = 0; i < pieceQuantity; i++)
        {
            // Calculate the position of the current object
            Vector3 spawnPosition = new Vector3(surfaceStartPoint + (i * segmentLenght), playerSurface.transform.position.y, playerSurface.transform.position.z);
            posList.Add(spawnPosition);

        }

        return posList;
    }

    [Separator("HANDS")] //both players have them but for now i will only care about this.
    [SerializeField] PlayerHand hand_1;
    [SerializeField] PlayerHand hand_2;


    public void PlayerHand_GrabIt(Magnetic magnetic)
    {
        //the closest grab it
        float distance_1 = Vector3.Distance(hand_1.transform.position, magnetic.transform.position);
        float distance_2 = Vector3.Distance(hand_2.transform.position, magnetic.transform.position);

        if (distance_1 < distance_2)
        {
            //this means the hand 1 goes
            hand_1.GrabIt(magnetic);
            hand_2.BecomeIdle();
        }
        else
        {
            //this means the hand 2 goes.
            hand_2.GrabIt(magnetic);
            hand_1.BecomeIdle();
        }


    }


    public void PlayerHand_PlaceIt(Vector3 pos)
    {
        hand_1.PlaceIt(pos);
        hand_2.PlaceIt(pos);
    }
}
