using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPanel : MonoBehaviour {

    public InventoryBase inventory; //The inventory this ui object is attached to.
    private int lastWidth;
    private int lastHeight;
    public Canvas HUDCanvas;//The canvas we are drawing the UI on.

    public RectTransform gridContainer; //The container in which the grid for the UI interface is contained.
    public GridLayoutGroup gridLayout; //The layout component for the grid container.
    public GameObject gridImagePrefab; //The prefab we're using for each tile in the grid.
    public GameObject inventoryItemUIPrefab;

    [HideInInspector]
    public CanvasGroup panelCanvasGroup; //The canvas group for the inventory panel, which we're using to change its visability.

    private Rect containerRect;

    public float GridCellSize { get; set; } //We get the size of each cell on the grid here.
    public bool isOpen { get; set; }

    public void Start()
    {
        isOpen = false;
        panelCanvasGroup = GetComponent<CanvasGroup>();
        panelCanvasGroup.alpha = 0;
        panelCanvasGroup.interactable = false;
    }

    public void Update()
    {
    }

    public void Open()
    {
        isOpen = true;
        panelCanvasGroup.alpha = 1;
        panelCanvasGroup.interactable = true;

        if (inventory.invData.width != lastWidth || inventory.invData.height != lastHeight)
        {
            DestroyOldUIGrid();
            CreateUIGrid();

            lastWidth = inventory.invData.width;
            lastHeight = inventory.invData.height;
        }

        UpdateInventoryUI();
    }


    public void Close()
    {
        isOpen = false;
        panelCanvasGroup.alpha = 0;
        panelCanvasGroup.interactable = false;
    }

    public void DestroyOldUIGrid()
    {
        foreach (Transform child in gridContainer)
        {
            Destroy(child.gameObject); 
        }
    }

    public void DestroyOldItemUI()
    {

    }

    public void CreateUIGrid()
    {
        containerRect = RectTransformUtility.PixelAdjustRect(gridContainer, HUDCanvas);

        GridCellSize = (containerRect.width - gridLayout.spacing.x * (inventory.invData.width - 1)) / inventory.invData.width; //We're going to use this to set the size of each 'cell' element of the grid. Then we'll just have it hooked up to a scroll bar so that it can scroll down if we need too.
        gridLayout.cellSize = new Vector2(GridCellSize, GridCellSize);

        GameObject gridImage;
        for (int i = 0; i < (inventory.invData.width * inventory.invData.height); i++)
        {
            gridImage = Instantiate(gridImagePrefab, gridContainer, false);
        }
    }

    public void UpdateInventoryUI()
    {
        containerRect = RectTransformUtility.PixelAdjustRect(gridContainer, HUDCanvas);

        GameObject ItemUI;
        foreach (ItemBase invItem in inventory.inventoryList)
        {
            ItemUI = Instantiate(inventoryItemUIPrefab, gridContainer, false);

            float xpos = invItem.inventoryPosition.x * GridCellSize + Mathf.Max(invItem.inventoryPosition.x - 1, 0) * gridLayout.spacing.x; //Have to include the spacing between different cells for positioning.
            float ypos = invItem.inventoryPosition.y * GridCellSize + Mathf.Max(invItem.inventoryPosition.y - 1, 0) * gridLayout.spacing.y;

            ItemUI.transform.localPosition = new Vector2(xpos, -ypos); //The ypos is negative here due to unity being silly.

            float xSize = invItem.inventoryDimensions.x * GridCellSize + Mathf.Max(invItem.inventoryDimensions.x - 1, 0) * gridLayout.spacing.x;
            float ySize = invItem.inventoryDimensions.y * GridCellSize + Mathf.Max(invItem.inventoryDimensions.y - 1, 0) * gridLayout.spacing.y;
            print("X Size: " + xSize);
            ItemUI.GetComponent<RectTransform>().sizeDelta = new Vector2(xSize, ySize);
            ItemUI.GetComponent<InventoryItemUI>().invDimensions = invItem.inventoryDimensions;
            ItemUI.GetComponent<InventoryItemUI>().invPosition = invItem.inventoryPosition;
            ItemUI.GetComponent<InventoryItemUI>().invPanel = this;
        }
    }

    public ItemBase GetItemAtPosition( Vector2 pos )
    {
        foreach (ItemBase invItem in inventory.inventoryList)
        {
            if (invItem.inventoryPosition == pos )
            {
                return invItem;
            }
        }
        return null;
    }
}
