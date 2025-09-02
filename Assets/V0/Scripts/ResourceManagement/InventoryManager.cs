using UnityEngine;
using Studio23.SS2.InventorySystem.Core;
using Studio23.SS2.InventorySystem.Data;
using System.IO;

public class InventoryManager : MonoBehaviour
{

    public InventoryHolder playerInventoryHolder;

    private string _savePath;

    private void Awake()
    {

        _savePath = Path.Combine(Application.persistentDataPath, "inventory.json");
        Debug.Log("Save file location: " + _savePath);
}

    [ContextMenu("Save Inventory")]
    public void SaveInventory()
    {

        var saveData = playerInventoryHolder.Backpack.GetInventorySaveData();

        var wrapper = new InventorySaveWrapper { SavedItems = saveData };

        string json = JsonUtility.ToJson(wrapper, true);

        File.WriteAllText(_savePath, json);
        Debug.Log($"Inventory saved to {_savePath}");
    }

    [ContextMenu("Load Inventory")]
    public void LoadInventory()
    {
        if (!File.Exists(_savePath))
        {
            Debug.LogWarning("No inventory save file found!");
            return;
        }

        string json = File.ReadAllText(_savePath);

        var wrapper = JsonUtility.FromJson<InventorySaveWrapper>(json);

        playerInventoryHolder.Backpack.LoadInventoryData(wrapper.SavedItems);

        Debug.Log("Inventory loaded!");
    }
}