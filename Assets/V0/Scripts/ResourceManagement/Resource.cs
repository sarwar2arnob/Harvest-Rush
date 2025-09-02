using UnityEngine;
using DG.Tweening;
using Studio23.SS2.InventorySystem.Core;
using Studio23.SS2.InventorySystem.Data;

public class Resource : MonoBehaviour
{
    [Header("Resource Settings")]
    [SerializeField] private string ToolUsed;

    [Header("Animation Settings")]
    [SerializeField] private float bobHeight = 0.05f;
    [SerializeField] private float bobDuration = 0.25f;

    [Header("Inventory Settings")]
    [Tooltip("The ScriptableObject item that is given to the player's inventory.")]
    [SerializeField] private ItemBase itemToGive;

    private ResourceData resourceData;

    public void InitializeResource(ResourceData data)
    {
        this.resourceData = data;
        transform.DOKill();
        Vector3 currentPosition = transform.position;
        transform.DOMoveY(currentPosition.y + bobHeight, bobDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }

    private void OnDisable()
    {
        transform.DOKill();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (itemToGive == null || !other.CompareTag(ToolUsed)) return;

        var inventoryHolder = other.GetComponentInParent<InventoryHolder>();

        if (inventoryHolder != null)
        {

            inventoryHolder.Backpack.AddItem(itemToGive);
            Debug.Log($"Collected {itemToGive.name} and added it to the Backpack.");

            if (resourceData != null)
            {
                if (resourceData.collectionSound != null)
                {

                    AudioManager.Instance.PlaySFX(resourceData.collectionSound);
                }
                if (resourceData.collectionEffect != null)
                {

                    EffectManager.Instance.PlayEffect(resourceData.collectionEffect, transform.position);
                }
            }
            gameObject.SetActive(false);
        }
    }
}