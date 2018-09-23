using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : InventoryBase {

    public List<ItemBase> testItems = new List<ItemBase>();
    public bool yeetums = false;

    protected override void Initialise()
    {
        GunBase item2 = gameObject.AddComponent<GunBase>();
        item2.gunData = Resources.Load("MP5") as GunScript;
        item2.InitialiseGunData();
        item2.inventoryPosition = new Vector2(2, 2);
        AddInventoryItemForced(item2);

        invData = new inventoryData(9, 4, 10);
        for (int i = 0; i < 5; i++)
        {
            GunBase item = gameObject.AddComponent<GunBase>();
            item.gunData = Resources.Load("MP5") as GunScript;
            item.InitialiseGunData();
            testItems.Add(item);
        }
    }

    protected void Update()
    {
        if(!yeetums)
        {
            foreach (ItemBase item in testItems)
            {
                print(item.itemName);
                AddInventoryItem(item);
            }
            yeetums = true;
        }

    }

}
