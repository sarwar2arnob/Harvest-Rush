using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerStat : MonoBehaviour
{
    public float MaxValue = 100f;
    public float WaitTime = 10f;

    [SerializeField] protected float currentValue;

    [Header("UI Reference")]
    public Image uiBar;

    protected virtual void Start()
    {
        currentValue = MaxValue;
        UpdateUI();

        StartCoroutine(AutoReset());
    }

    public void Change(float amount)
    {
        currentValue += amount;
        currentValue = Mathf.Clamp(currentValue, 0, MaxValue);
        UpdateUI();
    }

    public float GetValue()
    {
        return currentValue;
    }

    private void UpdateUI()
    {
        if (uiBar != null)
        {
            uiBar.fillAmount = currentValue / MaxValue;
        }
    }

    private IEnumerator AutoReset()
    {
        while (true)
        {
            yield return new WaitForSeconds(WaitTime); 
            currentValue = MaxValue;              
            UpdateUI();
            Debug.Log($"{gameObject.name} reset to {MaxValue}");
        }
    }
}