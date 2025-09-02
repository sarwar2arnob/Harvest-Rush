using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class PlayerHealth : PlayerStat
{
    [Header("UI References")]
    public GameObject DefeatBackground;

    [Header("Scene Reset Settings")]
    public float ResetDelay = 1f; 

    protected override void Start()
    {
        base.Start();
        if (DefeatBackground != null)
            DefeatBackground.SetActive(false);
    }

    public void TakeDamage(int amount)
    {
        Change(-amount);
        Debug.Log("Player Health: " + GetValue());

        if (GetValue() <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player Died!");

        if (DefeatBackground != null)
            DefeatBackground.SetActive(true);

        gameObject.SetActive(false);

        StartCoroutine(ResetSceneAfterDelay());
    }

    private IEnumerator ResetSceneAfterDelay()
    {
        yield return new WaitForSeconds(ResetDelay);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}