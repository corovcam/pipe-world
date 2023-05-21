using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public GameObject inventoryGO;

    public GameObject inventorySlotPrefab;
    public GameObject inventoryItemPrefab;

    public void AddPipeToInventory(Pipe pipe, GameObject pipePrefab)
    {
        GameObject slot = Instantiate(inventorySlotPrefab, inventoryGO.transform);
        GameObject item = Instantiate(inventoryItemPrefab, slot.transform);

        item.GetComponent<Image>().sprite = pipePrefab.GetComponent<SpriteRenderer>().sprite;
        item.GetComponent<DraggableItem>().pipePrefab = pipePrefab;
        item.GetComponent<DraggableItem>().pipeType = pipe;
    }

    public void RemovePipeFromInventory(GameObject slotWithItemGO)
    {
        Destroy(slotWithItemGO);
    }

    public void SetInventoryVisibilityTo(bool active)
    {
        inventoryGO.SetActive(active);
    }
}
