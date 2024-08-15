using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystemComponent 
{
    //i want to hold information 
    

    
    
    
        
}

//this exists here just for a easy template.
public class ItemData_TEMPLATE : ScriptableObject
{
    [field: SerializeField] public string itemName { get; private set; }

    [TextArea] public string itemDescription;
    [field: SerializeField] public Sprite itemSprite { get; private set; }
    [field: SerializeField] public ItemType_TEMPLATE itemType { get; private set; }
    [field: SerializeField] public int itemSize { get; private set; } = 1;
}

public enum ItemType_TEMPLATE
{
    
}


public class ItemClass_TEMPLATE
{
    //this will bejust a bunch of stuff to more easily create inventory systems.

}