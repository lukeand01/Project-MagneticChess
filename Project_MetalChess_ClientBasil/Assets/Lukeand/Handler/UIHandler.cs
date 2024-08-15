using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    //WE STORY REFERENCES HERE OF THE UI. THATS IT

    public static UIHandler instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    [field:SerializeField] public UI_Round _ui_Round {  get; private set; }



}
