using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InventoryBase : MonoBehaviour {

    public List<ItemBase> inventoryList = new List<ItemBase>();
    public inventoryData invData;

    public InventoryPanel inventoryUIPanel; //We can attach a UI object to our inventory panel.

    public bool isOpen
    {
        get
        {
            return inventoryUIPanel != null ? inventoryUIPanel.isOpen : false;
        }
        set
        {
            if (inventoryUIPanel != null)
            {
                inventoryUIPanel.isOpen = value;
            }
            else
            {
                Debug.LogWarning("Trying to get the value of whether an inventory panel is open that doesn't exist.");
            }
        }
    }

    void Start () {
        InitialiseInternal();
        Initialise();
	}
	
	void Update () {		
	}

    public void Open()
    {
        if (inventoryUIPanel != null)
        {
            inventoryUIPanel.Open();
        }
    }

    public void Close()
    {
        if(inventoryUIPanel != null)
        {
            inventoryUIPanel.Close();
        }
    }

    protected virtual void InitialiseInternal()
    {
      invData = new inventoryData(10, 10, 10);
    }

    protected virtual void Initialise()
    {

    }

    public void LoadInventoryData() //We can use this to store data about what is stored in containers/backpacks/etc.
    {
        //TODO
        //... :P
    }

    public void SaveInventoryData()//Save what's stored into a file.
    {
    }

    public Vector2 FindFirstFreeSpotForItem( ItemBase item )
    {
        for (int xPos = 1; xPos <= invData.width; xPos++)
        {
            
        }
        return new Vector2(0, 0);
    }

    public void SortInventory()
    {

    }

    public void AddInventoryItem( ItemBase item )
    {
        inventoryList.Add(item);
        item.inventoryPosition = new Vector2(2, 3); // FindFirstFreeSpotForItem( item );
    }

    public void RemoveInventoryItem()
    {

    }

    public void MoveItem( ItemBase item, Vector2 newpos )
    {

    }

    public struct inventoryData
    {
        public int width, height;
        public float weightLimit;

        public inventoryData( int w, int h, float wLimit )
        {
            width = w;
            height = h;
            weightLimit = wLimit;
        }
        
    }
}
