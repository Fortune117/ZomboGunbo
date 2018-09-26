using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Basic Item", menuName = "Basic Item")]
public class ItemDataBase : ScriptableObject {

    [Header("Item Information")]
    public string itemName;
    public string itemDescription;
    public string itemType;
    public int itemRarity;
    public float itemWeight;

    [Space]
    [Header("Inventory Information")]
    public bool itemCanStack;
    public int itemMaxStackSize;
    public Sprite itemInventoryImage;
    public Sprite gameObjectImage;
    public Vector2 inventoryDimesions;

    [HideInInspector]
    public bool valid = false;

    public void OnValidate()
    {
        valid = false;
    }

}
