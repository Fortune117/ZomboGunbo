using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemBase : MonoBehaviour {

    /* How exactly do we want to do items?
     * Well, I don't want level restrictions on items. I think, a large open world where it gets progressively more dangerous the further out you go from your starting base would be ideal.
     * The game should be sufficiently skill based such that you can go to some of the most dangerous starting areas at level 1. I want people to be able to use the loot they find there, that way the feel rewarded.
     * Should we have rarities on items? Yeah, I think so. Making items of different rarities means that you're more likely to be interested in looking for good items. Don't want to make this a grind though. 
     * So really rare items should probably be pregenerated and just placed on the map. I think the highest tier should be pregen, maybe placed in different locations though on each playthrough.
     * Player should still be able to find cool shit while looting. I like the idea of special equipment and what not, maybe make some things you can find give benefits to stuff like that.
     * 
     * 
     * 
     * 
     */

    public string itemName { get; set; } //The actual diplay name of the item.

    public string itemDescription { get; set; } //Description of the Item. We can use this for either info or flavour text.

    public string itemType { get; set; } //What type of item is it? Food, weapon, gun, axe, etc.

    public int itemRarity { get; set; } //We will give items an integer rarity, between 1-4. 1 is common, 2 is uncommon, 3 is rare, 4 is extrordinary, 5 is exotic

    public float itemWeight { get; set; } //We may have a carrying capacity so it's important that items have a weight.

    public bool itemCanStack { get; set; } //Whether or not the item can stack in the inventory.

    public int itemMaxStackSize { get; set; } //The maximum size for a stack of items.


	// Use this for initialization
	protected virtual void Start () {

        itemName = "Base Item";
        itemDescription = "You shouldn't be reading this. If you are, Fortune fucked up.";
        itemType = "base item type";
        itemRarity = 5; //Better beleive this shit is exotic. No one should ever have it.
        itemWeight = 0F;
        itemCanStack = false;
        itemMaxStackSize = 1;

        ItemInitialiseInternal(); //We can use these so we don't need to be overwritng the start function.
        ItemInitialise();
    }

    protected virtual void ItemInitialiseInternal() { }
    protected virtual void ItemInitialise() { }

    protected void Update()
    {
        ItemThinkInternal();    //These two methods will be used for each item.
        ItemThink();            //We will need them for some base stuff that all items will need. I have no idea what, but I guess it's useful to have.
    }

    protected virtual void ItemThinkInternal() { }
    protected virtual void ItemThink() { }

    public virtual void ItemDisplayInfo() { } //We will use this to display information about the item.

    public virtual void ItemUse() { } //When we want to 'use' the item. So for food, it could be eating, for a gun it could be equipping it, etc.

}
