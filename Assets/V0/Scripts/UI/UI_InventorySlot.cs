using UnityEngine;
using UnityEngine.UI;
using Studio23.SS2.InventorySystem.Data;
using TMPro;

public class UI_InventorySlot : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Image _itemIcon;
    [SerializeField] private TextMeshProUGUI _itemCountText;

    private Item _currentItem;

    public void SetItem(Item item, int count)
    {
        _currentItem = item;
        UpdateSlotUI(count);
    }

    private void UpdateSlotUI(int count)
    {
        if (_currentItem != null)
        {
            _itemIcon.sprite = _currentItem.Icon;
            _itemIcon.enabled = true;

            if (count > 1)
            {
                _itemCountText.text = count.ToString();
                _itemCountText.gameObject.SetActive(true);
            }
            else
            {
                _itemCountText.gameObject.SetActive(false);
            }
        }
        else
        {
            _itemIcon.sprite = null;
            _itemIcon.enabled = false;
            _itemCountText.gameObject.SetActive(false);
        }
    }
}