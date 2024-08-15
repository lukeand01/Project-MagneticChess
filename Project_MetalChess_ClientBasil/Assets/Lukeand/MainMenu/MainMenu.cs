using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void Call_SinglePlayer()
    {



        GameHandler.instance._sceneLoader.LoadGame(SceneType.Singleplayer);

    }

    public void Call_Quit()
    {
        Application.Quit();
    }
}
