using System.Collections.Generic;
using UnityEngine;
using Studio23.SS2.InventorySystem.Data;
using Studio23.SS2.InventorySystem.Core;

public class UI_InventoryDisplay : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private InventoryHolder targetInventoryHolder;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Transform slotContainer;

    public static UI_InventoryDisplay Instance;

    private List<GameObject> _instantiatedSlots = new List<GameObject>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }
    void OnEnable()
    {
        if (targetInventoryHolder != null)
        {
            targetInventoryHolder.Backpack.OnItemAdded += HandleItemAdded;
            targetInventoryHolder.Backpack.OnItemRemoved += HandleItemRemoved;
            Redraw();
        }
    }

    void OnDisable()
    {
        if (targetInventoryHolder != null)
        {
            targetInventoryHolder.Backpack.OnItemAdded -= HandleItemAdded;
            targetInventoryHolder.Backpack.OnItemRemoved -= HandleItemRemoved;
        }
    }

    public void Redraw()
    {

        foreach (GameObject slot in _instantiatedSlots)
        {
            Destroy(slot);
        }
        _instantiatedSlots.Clear();

        if (targetInventoryHolder == null || targetInventoryHolder.Backpack == null) return;

        var itemCounts = new Dictionary<ItemBase, int>();
        foreach (ItemBase item in targetInventoryHolder.Backpack.GetAll())
        {
            if (itemCounts.ContainsKey(item))
            {
                itemCounts[item]++;
            }
            else
            {
                itemCounts[item] = 1;
            }
        }

        foreach (var itemGroup in itemCounts)
        {
            CreateSlotForItem(itemGroup.Key, itemGroup.Value);
        }
    }

    private void CreateSlotForItem(ItemBase item, int count)
    {
        GameObject newSlot = Instantiate(slotPrefab, slotContainer);
        var slotScript = newSlot.GetComponent<UI_InventorySlot>();

        if (item is Item basicItem)
        {
            slotScript.SetItem(basicItem, count);
        }

        _instantiatedSlots.Add(newSlot);
    }

    private void HandleItemAdded(ItemBase item)
    {
        Debug.Log($"UI: Item {item.Name} added. Redrawing.");
        Redraw();
    }

    private void HandleItemRemoved(ItemBase item)
    {
        Debug.Log($"UI: Item {item.Name} removed. Redrawing.");
        Redraw();
    }
}