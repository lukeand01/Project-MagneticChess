using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinglePlayerHandler : MonoBehaviour
{

    GameHandler handler;
    [SerializeField] int amountOfMagnets;
    [SerializeField] MagnetPieceGraphicType[] magneticArray;
    private void Awake()
    {
        handler = GetComponent<GameHandler>();
    }


    public void StartSinglePlayer()
    {
        //we start the bottom place 
        //put the camera there.
        //start a timer
        //there is no round system
        //it counts point every time it does something
        //when the timer reachs 0 it stops.
        //save the score somewhere.



        List<Vector3> positionForMagnetList = handler.Player_1.GetPositionsToAllignMagnetPieces( amountOfMagnets);

        for (int i = 0; i < positionForMagnetList.Count; i++)
        {
            Magnetic newObject = handler._pool_Main.GetMagnet();
            AddMagnet(newObject);
            newObject.SetHand(positionForMagnetList[i]);

            int random = Random.Range(0, magneticArray.Length);
            MagnetPieceGraphicType magnetGraphic = magneticArray[random];

            newObject.SetGraphic(magnetGraphic);
        }

        //then we start the timer.
        StartCoroutine(StartTimerProcess());


    }



    IEnumerator StartTimerProcess()
    {

        TimeClass time = new TimeClass(0, 20);

        UIHandler.instance._ui_Round.StartRound();
        UIHandler.instance._ui_Round.UpdateTimerText(time);

        yield return new WaitForSeconds(0.5f);

        int safeBreak = 0;

        while(!time.IsTimerZero())
        {

            safeBreak++;

            if(safeBreak > 10000)
            {
                break;
            }

            time.CountTimerDown();

            UIHandler.instance._ui_Round.UpdateTimerText(time);
            yield return new WaitForSeconds(1);
        }

        yield return new WaitForSeconds(1);

        GameHandler.instance._sceneLoader.LoadMainMenu();

    }

    List<Magnetic> magnetList = new(); 

    public void AddMagnet(Magnetic magnet)
    {
        magnetList.Add(magnet);
    }
    public void RemoveMagnet(Magnetic magnet)
    {
        for (int i = 0; i < magnetList.Count; i++)
        {
            if (magnetList[i].name == magnet.name)
            {
                magnetList.RemoveAt(i);

                return;
            }
        }
    }

    public void CheckIfShouldWin()
    {
        if(magnetList.Count <= 0)
        {
            Debug.Log("victory");
            StopAllCoroutines();
            GameHandler.instance._sceneLoader.LoadMainMenu();
        }
    }




}


//wind manager
//renderloop
//zip background
//timeforupdate.waitforlastpresentationandupdate
//how to imrpove the perfomance


//casual running game
//it needs to be browser.
//the options that spawn are memes.
//


//authorative server. 


//infinite project
//