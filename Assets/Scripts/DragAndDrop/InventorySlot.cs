using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            var item = eventData.pointerDrag.GetComponent<DraggableItem>();
            if (item != null)
            {
                item.parentAfterDrag = transform;
            }
        }
    }
}
