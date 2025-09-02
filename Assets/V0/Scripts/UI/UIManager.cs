using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Studio23.SS2.InventorySystem.Data;

public class UIManager : MonoBehaviour
{
    [Header("Game State UI")]
    [SerializeField] private TextMeshProUGUI dayText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI dayEndText;

    [Header("Resource Requirements")]
    [SerializeField] private TextMeshProUGUI requiredFoodText;
    [SerializeField] private TextMeshProUGUI requiredWoodText;
    [SerializeField] private TextMeshProUGUI requiredStoneText;

    [Header("Collected Resources")]
    [SerializeField] private TextMeshProUGUI collectedFoodText;
    [SerializeField] private TextMeshProUGUI collectedWoodText;
    [SerializeField] private TextMeshProUGUI collectedStoneText;
    [SerializeField] private GameObject pop_UP;

    [Header("Tool UI")]
    [SerializeField] private Image currentToolIcon;

    [Header("Inventory Connection")]
    [Tooltip("Drag the Player object with the InventoryHolder component here.")]
    [SerializeField] private InventoryHolder targetInventoryHolder;

    private GameManager gameManager;

    private void OnEnable()
    {
        if (targetInventoryHolder != null)
        {
            targetInventoryHolder.Backpack.OnItemAdded += HandleInventoryChange;
            targetInventoryHolder.Backpack.OnItemRemoved += HandleInventoryChange;
        }
        PlayerController.OnToolSwitched += HandleToolSwitchUI;
    }

    private void OnDisable()
    {
        if (targetInventoryHolder != null)
        {
            targetInventoryHolder.Backpack.OnItemAdded -= HandleInventoryChange;
            targetInventoryHolder.Backpack.OnItemRemoved -= HandleInventoryChange;
        }

        PlayerController.OnToolSwitched -= HandleToolSwitchUI;
    }

    void Start()
    {
        gameManager = GameManager.Instance;
        UpdateResourceCounts();
    }

    void Update()
    {
        if (gameManager == null) return;

        dayText.text = "" + gameManager.CurrentDay;
        timerText.text = "" + Mathf.CeilToInt(gameManager.TimeLeftInDay);
        requiredFoodText.text = gameManager.RequiredFood.ToString();
        requiredWoodText.text = gameManager.RequiredWood.ToString();
        requiredStoneText.text = gameManager.RequiredStone.ToString();

    }

    private void HandleInventoryChange(ItemBase item)
    {
        UpdateResourceCounts();
    }
    private void UpdateResourceCounts()
    {
        if (targetInventoryHolder == null) return;

        int foodCount = 0;
        int woodCount = 0;
        int stoneCount = 0;
        foreach (var item in targetInventoryHolder.Backpack.GetAll())
        {

            switch (item.Id)
            {
                case "food":
                    foodCount++;
                    break;
                case "wood":
                    woodCount++;
                    break;
                case "stone":
                    stoneCount++;
                    break;
            }
        }


        collectedFoodText.text = foodCount.ToString();
        collectedWoodText.text = woodCount.ToString();
        collectedStoneText.text = stoneCount.ToString();
    }
    private void HandleToolSwitchUI(ToolData data)
    {
        if (currentToolIcon != null && data != null)
        {
            currentToolIcon.sprite = data.toolIcon;
        }
    }

    public void ShowDayEndMessage(int day)
    {
        if (dayEndText != null)
        {
            pop_UP.SetActive(true);
            dayEndText.gameObject.SetActive(true);
            dayEndText.text = $"Congratulations! You have survived Day {day}.";
        }
    }

    public void HideDayEndMessage()
    {
        if (dayEndText != null)
        {
            pop_UP.SetActive(false);
            dayEndText.gameObject.SetActive(false);
        }
    }
}