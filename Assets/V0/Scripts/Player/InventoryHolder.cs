using UnityEngine;
using Studio23.SS2.InventorySystem.Core;
using Studio23.SS2.InventorySystem.Data;

public class InventoryHolder : MonoBehaviour
{
    public InventoryBase<ItemBase> Backpack;

    void Awake()
    {
        Backpack = new InventoryBase<ItemBase>("Backpack", false);
    }
}