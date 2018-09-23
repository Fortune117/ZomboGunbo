using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class InventoryItemUI : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler {

    public InventoryPanel invPanel;
    public ItemBase parentItem;

    public Vector2 invPosition;
    public Vector2 invDimensions;

    public Vector2 oldPos;
    public Transform oldParent;

    public bool clicked = false;


    public void OnBeginDrag(PointerEventData eventData)
    {
        oldPos = transform.localPosition;
        oldParent = invPanel.gridContainer;

        Canvas[] c = GetComponentsInParent<Canvas>();
        Canvas topmost = c[c.Length - 1];

        transform.SetParent(topmost.transform);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Canvas[] c = GetComponentsInParent<Canvas>();
        Canvas topmost = c[c.Length - 1];
        Rect screenRect = RectTransformUtility.PixelAdjustRect(gameObject.GetComponent<RectTransform>(), topmost);

        Vector3 mPosScreen = Input.mousePosition;
        Vector3 targPos = new Vector3(mPosScreen.x - screenRect.width / 2, mPosScreen.y + screenRect.height / 2);

        Vector3 mPos = Camera.main.ScreenToWorldPoint(targPos);

        Vector2 pos2D = new Vector2(mPos.x, mPos.y);
        transform.position = pos2D;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Canvas[] c = GetComponentsInParent<Canvas>();
        Canvas topmost = c[c.Length - 1];
        Rect screenRect = RectTransformUtility.PixelAdjustRect(gameObject.GetComponent<RectTransform>(), topmost);

        Vector3 mPosScreen = Input.mousePosition;
        Vector3 targPos = new Vector3(mPosScreen.x - screenRect.width / 2, mPosScreen.y + screenRect.height / 2);

        Rect gridContainer = RectTransformUtility.PixelAdjustRect(invPanel.gridContainer,topmost);
        float gridWidth = gridContainer.width;
        float gridHeight = gridContainer.height;

        GridLayoutGroup gridLayout = invPanel.gridContainer.GetComponent<GridLayoutGroup>();
        float cellWidth = gridLayout.cellSize.x;
        float cellHeight = gridLayout.cellSize.y;

        float cellGap = gridLayout.spacing.x; //Not sure if we'll need to consider this for positioning items in the inventory.
        //In theory, it may make a difference of like 5ish pixels for choosing which slot to put something in.
        //But I doubt anyone will notice.

        Vector3 gridContainerScreenPos = Camera.main.WorldToScreenPoint(invPanel.gridContainer.position);

        float xMod = (float)Math.Round((targPos.x - gridContainerScreenPos.x) / cellWidth); //X position in terms of the inventory grid.
        float yMod = -(float)Math.Round((targPos.y - gridContainerScreenPos.y) / cellHeight);//Y position in terms of the inventory grid.

        Vector2 invTargPos = new Vector2(xMod, yMod);

        print(xMod);
        print(yMod);

        transform.SetParent(oldParent);

        if (xMod > invPanel.inventory.invData.width - 1 || yMod > invPanel.inventory.invData.height - 1 || xMod < 0 || yMod < 0) //If we move the item to outside our inventory screen, we should drop it.
        {
            invPanel.inventory.DropInventoryItem(parentItem);
            invPanel.ReloadUI();
            return;
        }

        if (!invPanel.inventory.ItemCanFitAtPosition(parentItem, invTargPos))
        {
            transform.localPosition = oldPos;
            return;
        }

        parentItem.inventoryPosition = invTargPos;

        invPanel.ReloadUI();
    }

    public void Update()
    {
    }
}
