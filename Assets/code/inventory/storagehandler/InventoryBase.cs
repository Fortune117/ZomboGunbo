using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

    //We're going to automaticallly add items to the inventory using this function.
    //We want to sort from left to right, filling coloumns first.
    public Vector2 FindFirstFreeSpotForItem( ItemBase item ) 
    {
        int maxXPos = invData.width - 1;
        int maxYPos = invData.height - 1;

        bool[,] takenSpaces = GetInventoryTakenSpacesMatrix();

        int itemWidth = (int)item.inventoryDimensions.x;
        int itemHeight = (int)item.inventoryDimensions.y;

        for (int xPos = 0; xPos < invData.width; xPos++)
        {
            for (int yPos = 0; yPos < invData.height; yPos++)  
            {
                if (!takenSpaces[xPos,yPos]) //If the space at xPos,yPos on the inventory grid isn't taken, check to see if the item fits.
                {
                    for (int xWidth = 0; xWidth < itemWidth; xWidth++) //Since items must be at least 1x1, we could skip the check at 0. But! If we are only 1 x unit across, it would skip the y check, when y could be larger than 1. So start at 0.
                    {
                        for (int yHeight = 1; yHeight < itemHeight; yHeight++) //Now here, it doesn't matter.
                        {
                            if(xPos + xWidth > maxXPos || yPos + yHeight > maxYPos)
                            {
                                goto DidntFit;
                            }

                            if (takenSpaces[xPos+xWidth,yPos+yHeight]) //If one of the spots we'd need to occupy is taken, then we get the heckle out of dodge.
                            {
                                goto DidntFit;
                            }

                            if (xWidth == itemWidth-1 && yHeight == yHeight-1)
                            {
                                return new Vector2(xPos, yPos);
                            }
                        }
                    }
                    return new Vector2(xPos, yPos); //If we skipped the loops for dimensions, that means our dimensions are one so we must fit.
                }
                DidntFit:;
            }
        }
        print("Inventory is Full");
        return -Vector2.one;
    }

    public void SortInventory()
    {

    }

    public void AddInventoryItem( ItemBase item )
    {
        Vector2 invPos = FindFirstFreeSpotForItem(item);
        if (!(invPos == -Vector2.one))
        {
            item.inventoryPosition = invPos;
            inventoryList.Add(item);
        }
    }

    public void AddInventoryItemForced( ItemBase item )
    {
        if(item.inventoryPosition == null)
        {
            item.inventoryPosition = new Vector2(0, 0);
        }
        inventoryList.Add(item);
    }

    public void RemoveInventoryItem( ItemBase item )
    {
        inventoryList.Remove(item);
    }

    public void DropInventoryItem( ItemBase item )
    {
        RemoveInventoryItem(item);
    }

    public void MoveItem( ItemBase item, Vector2 newpos )
    {

    }

    public bool[,] GetInventoryTakenSpacesMatrix()
    {
        bool[,] takenSpaces = new bool[invData.width, invData.height];

        int itemWidth, itemHeight;
        int itemX, itemY;

        foreach (ItemBase invItem in inventoryList)
        {
            int maxXPos = invData.width - 1;
            int maxYPos = invData.height - 1;

            Vector2 itemPos = invItem.inventoryPosition;
            itemX = (int)itemPos.x;
            itemY = (int)itemPos.y;
            itemWidth = (int)invItem.inventoryDimensions.x;
            itemHeight = (int)invItem.inventoryDimensions.y;

            if ((invItem.inventoryPosition.x < 0 || invItem.inventoryPosition.y < 0) || (invItem.inventoryPosition.x + invItem.inventoryDimensions.x - 1 > maxXPos || invItem.inventoryPosition.y + invItem.inventoryDimensions.y - 1 > maxYPos))
            {
                continue;
            }

            for (int x = 0; x < itemWidth; x++)
            {
                for (int y = 0; y < itemHeight; y++)
                {
                    takenSpaces[itemX + x, itemY + y] = true;
                }
            }
        }
        return takenSpaces;
    }

    public bool[,] GetInventoryTakenSpacesMatrix(ItemBase filterItem) //We'll add an overload to this so that we can ignore items in this table if we need too.
    {
        bool[,] takenSpaces = new bool[invData.width, invData.height];

        int itemWidth, itemHeight;
        int itemX, itemY;

        foreach (ItemBase invItem in inventoryList)
        {
            if (invItem == filterItem)
                continue;

            int maxXPos = invData.width - 1;
            int maxYPos = invData.height - 1;

            Vector2 itemPos = invItem.inventoryPosition;
            itemX = (int)itemPos.x;
            itemY = (int)itemPos.y;
            itemWidth = (int)invItem.inventoryDimensions.x;
            itemHeight = (int)invItem.inventoryDimensions.y;

            if ((invItem.inventoryPosition.x < 0 || invItem.inventoryPosition.y < 0) || (invItem.inventoryPosition.x + invItem.inventoryDimensions.x - 1 > maxXPos || invItem.inventoryPosition.y + invItem.inventoryDimensions.y - 1 > maxYPos))
            {
                continue;
            }

            for (int x = 0; x < itemWidth; x++)
            {
                for (int y = 0; y < itemHeight; y++)
                {
                    takenSpaces[itemX + x, itemY + y] = true;
                }
            }
        }
        return takenSpaces;
    }

    public bool[,] GetInventoryTakenSpacesMatrix(ItemBase[] filterItemList) //We'll add another overload for a table of items to fitler.
    {
        bool[,] takenSpaces = new bool[invData.width, invData.height];

        int itemWidth, itemHeight;
        int itemX, itemY;

        foreach (ItemBase invItem in inventoryList)
        {
            foreach (ItemBase filterItem in filterItemList)
                if (filterItem == invItem)
                    goto ItemWasFiltered;

            int maxXPos = invData.width - 1;
            int maxYPos = invData.height - 1;

            Vector2 itemPos = invItem.inventoryPosition;
            itemX = (int)itemPos.x;
            itemY = (int)itemPos.y;
            itemWidth = (int)invItem.inventoryDimensions.x;
            itemHeight = (int)invItem.inventoryDimensions.y;

            if ((invItem.inventoryPosition.x < 0 || invItem.inventoryPosition.y < 0) || (invItem.inventoryPosition.x + invItem.inventoryDimensions.x - 1 > maxXPos || invItem.inventoryPosition.y + invItem.inventoryDimensions.y - 1 > maxYPos))
            {
                continue;
            }

            for (int x = 0; x < itemWidth; x++)
            {
                for (int y = 0; y < itemHeight; y++)
                {
                    takenSpaces[itemX + x, itemY + y] = true;
                }
            }
            ItemWasFiltered:;
        }
        return takenSpaces;
    }

    public bool ItemCanFitAtPosition(ItemBase item, Vector2 pos)
    {
        bool[,] takenSpaces = GetInventoryTakenSpacesMatrix(item); //Filter out our item we're checking from the taken spots matrix.

        int maxXPos = invData.width - 1;
        int maxYPos = invData.height - 1;

        for (int xWidth = 0; xWidth < item.inventoryDimensions.x; xWidth++)
        {
            for (int yHeight = 0; yHeight < item.inventoryDimensions.y; yHeight++)
            {
                if ((int)pos.x + xWidth > maxXPos || (int)pos.y + yHeight > maxYPos || (int)pos.x < 0 || (int)pos.y < 0)
                {
                    return false;
                }

                if(takenSpaces[(int)pos.x + xWidth, (int)pos.y + yHeight])
                {
                    return false;
                }
            }
        }
        return true;
    }

    public ItemBase GetItemAtPosition(Vector2 pos)
    {
        foreach (ItemBase invItem in inventoryList)
        {
            int width = (int)invItem.inventoryDimensions.x - 1;
            int height = (int)invItem.inventoryDimensions.y - 1;
            if (pos.x >= invItem.inventoryPosition.x && pos.x <= invItem.inventoryPosition.x + width
                && pos.y >= invItem.inventoryPosition.y && pos.y <= invItem.inventoryPosition.y + height)
            {
                return invItem;
            }
        }
        return null;
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
