using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryItemUI : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler {

    public InventoryPanel invPanel;

    public Vector2 invPosition;
    public Vector2 invDimensions;

    public Vector2 oldPos;

    public bool clicked = false;

    public void OnBeginDrag(PointerEventData eventData)
    {
        oldPos = transform.localPosition;
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
        transform.localPosition = oldPos;
    }

    public void Update()
    {
    }
}
