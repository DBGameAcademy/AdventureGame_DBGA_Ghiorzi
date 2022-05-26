using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public Item Item { get; set; }
    public ItemSlot ParentSlot { get; private set; }
    public Image ItemImage { get; private set; }

    private Canvas _canvas = null;
    private RectTransform _rectTransform = null;

    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetParent(_canvas.transform);
        transform.SetAsLastSibling();
    }
    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        ItemSlot slot = null;

        foreach (RaycastResult result in results)
        {
            slot = result.gameObject.GetComponent<ItemSlot>();
            if (slot != null)
                break;
        }

        if (slot != null)
        {
            if (slot.SlotType == Item.eItemType.None || Item.Type == slot.SlotType)
            {
                slot.OnDrop(this);

                transform.SetParent(ParentSlot.transform);
                _rectTransform.anchoredPosition = Vector2.zero;
                return;
            }
        }

        // Out of inventory
        transform.SetParent(ParentSlot.transform);
        _rectTransform.anchoredPosition = Vector2.zero;
    }

    private void Awake()
    {
        ItemImage = GetComponent<Image>();
        ItemImage.enabled = false;
        _canvas = GetComponentInParent<Canvas>();
        _rectTransform = GetComponent<RectTransform>();
        ParentSlot = GetComponentInParent<ItemSlot>();  
    }

    
}
