using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyClass 
{


    public KeyClass()
    {
        SetUpKeys();
    }

    

    Dictionary<KeyType, KeyCode> keyDictionary = new Dictionary<KeyType, KeyCode>();



    public KeyCode GetKey(KeyType inputType)
    {
        return keyDictionary[inputType];
    }

    void ChangeKey(KeyType keyType, KeyCode key)
    {

    }

    
    void SetUpKeys()
    {
        keyDictionary.Add(KeyType.MoveLeft, KeyCode.A);
        keyDictionary.Add(KeyType.MoveUp, KeyCode.W);
        keyDictionary.Add(KeyType.MoveDown, KeyCode.S);
        keyDictionary.Add(KeyType.MoveRight, KeyCode.D);
        keyDictionary.Add(KeyType.Jump, KeyCode.Space);

        keyDictionary.Add(KeyType.StartRanged, KeyCode.LeftShift);

        keyDictionary.Add(KeyType.Interact, KeyCode.F);

        keyDictionary.Add(KeyType.Inventory, KeyCode.Tab);
    }


}

public enum KeyType
{     
    MoveLeft,
    MoveRight,
    MoveUp,
    MoveDown,
    Jump,
    StartRanged,
    Interact,
    Inventory

}