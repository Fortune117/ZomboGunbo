using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : InventoryBase {

    protected override void Initialise()
    {
        invData = new inventoryData(10, 10, 10);
    }

}
