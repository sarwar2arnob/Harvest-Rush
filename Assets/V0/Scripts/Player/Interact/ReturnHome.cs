using UnityEngine;

public class ReturnHome : MonoBehaviour, IInteract
{
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    public void Interact()
    {
        if (CanInteract())
        {
            Debug.Log("All resources collected! Ending the day and returning home.");
            gameManager.TimeLeftInDay = 0f;
        }
        else
        {
            Debug.Log("You must collect all required resources before you can end the day!");
        }
    }


    public bool CanInteract()
    {
        if (gameManager == null) return false;

        bool hasRequiredResources = gameManager.CollectedFood >= gameManager.RequiredFood &&
                                    gameManager.CollectedWood >= gameManager.RequiredWood &&
                                    gameManager.CollectedStone >= gameManager.RequiredStone;
        return hasRequiredResources;
    }

}