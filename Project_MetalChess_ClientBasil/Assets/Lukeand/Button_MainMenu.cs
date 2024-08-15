using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Button_MainMenu : ButtonBase
{
    //using this because the buttonevent is not working for some reason. this is fine for now


    //[SerializeField] MainMenu _menu;
    [SerializeField] SceneType _sceneType;
    public override void OnPointerClick(PointerEventData eventData)
    {
        //THIS EXISTS JUST AS A QUICK SOLUTION
        if(_sceneType == SceneType.Close)
        {
            Debug.Log("stuck " + _sceneType);
            Application.Quit();
            return;
        }

        Debug.Log("got here");
        base.OnPointerClick(eventData);
       
        GameHandler.instance._sceneLoader.LoadGame(_sceneType);
    }

}
