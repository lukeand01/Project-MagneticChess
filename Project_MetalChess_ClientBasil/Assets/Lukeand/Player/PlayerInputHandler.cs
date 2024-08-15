using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{

    bool isUsingMobileInputSystem;

    Camera _cam;
    LayerMask magnetPieceLayer;
    LayerMask tableLayer;

    Magnetic currentMagnetHolding;

    Player _player;

    public void UpdatePlayer(Player _player)
    {
        this._player = _player;
    }

    private void Awake()
    {
        _cam = Camera.main;
        magnetPieceLayer |= (1 << 6);
        tableLayer |= (1 << 7);
    }

    private void Update()
    {
        if (isUsingMobileInputSystem)
        {

        }
        else
        {
            PcInput();
        }
    }


    /// <summary>
    /// when we click we check if there is a magnetic. we call for it. the magnetuic itself decides if it can be interacted
    /// if it finds something then it will return to the "currentMagnetPiece"
    /// 
    /// </summary>

    void PcInput()
    {
        //here we check foir mouse input.

        if (currentMagnetHolding != null) Input_IsHoldingMagnet(); 
        else Input_NotHoldingMagnet ();
        

    }

    void Input_NotHoldingMagnet()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = _cam.ScreenPointToRay(Input.mousePosition);

            // Store the hit information
            RaycastHit hit;

            // Perform the raycast and filter by the target layer
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, magnetPieceLayer))
            {
                Magnetic magneticPiece = hit.collider.GetComponent<Magnetic>();
                if (magneticPiece == null) return;

                currentMagnetHolding = magneticPiece.TryToGetMagnetPiece();

                Debug.Log("got here");

                if(currentMagnetHolding != null)
                {
                    //
                    GameHandler.instance.Player_1.PlayerHand_GrabIt(currentMagnetHolding);

                }

            }
            else
            {
                
            }
        }
    }
    void Input_IsHoldingMagnet()
    {


        //we show the area where it will be placed.

        if (Input.GetMouseButtonDown(0))
        {
            //if we are holding something
            //and if we are detecting 
            Ray ray = _cam.ScreenPointToRay(Input.mousePosition);

            // Store the hit information
            RaycastHit hit;

            // Perform the raycast and filter by the target layer
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, tableLayer))
            {
                //we place the 
                if (currentMagnetHolding == null) Debug.Log("how did it got here?");


                //currentMagnetHolding.PositionToMove(hit.point);
                GameHandler.instance.Player_1.PlayerHand_PlaceIt(hit.point);
                //we are goiing to order
            }
            else
            {
                //we remove 
                
            }

            currentMagnetHolding = null;
        }
    }
    void MobileInput()
    {
        //here we check for touch
    }




}
