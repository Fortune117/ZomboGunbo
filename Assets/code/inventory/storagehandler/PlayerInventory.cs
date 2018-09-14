using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : InventoryBase {

    protected override void Initialise()
    {
        invData = new inventoryData(8, 5, 10);

        MP5 mp5 = this.gameObject.AddComponent<MP5>(); 
        AddInventoryItem(mp5);
    }

}
