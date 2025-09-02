using UnityEngine;

public class GameConditionsManager : MonoBehaviour
{
    private GameManager gameManager;
    private GameObject _player;

    private void Start()
    {
        gameManager = GameManager.Instance;
        _player = GameObject.FindGameObjectWithTag("Player");
        gameManager.OnNewDayStart.AddListener(HandleNewDayStart);
        gameManager.OnDayEnd.AddListener(HandleDayEnd);
    }

    public void CollectFood() => gameManager.CollectedFood++;
    public void CollectWood() => gameManager.CollectedWood++;
    public void CollectStone() => gameManager.CollectedStone++;

    public void HandleNewDayStart()
    {

        int dayIndex = gameManager.CurrentDay - 1;

        int currentMinFood = gameManager.Settings.MinFood + (dayIndex * gameManager.Settings.FoodIncreasePerDay);
        int currentMaxFood = gameManager.Settings.MaxFood + (dayIndex * gameManager.Settings.FoodIncreasePerDay);

        int currentMinWood = gameManager.Settings.MinWood + (dayIndex * gameManager.Settings.WoodIncreasePerDay);
        int currentMaxWood = gameManager.Settings.MaxWood + (dayIndex * gameManager.Settings.WoodIncreasePerDay);

        int currentMinStone = gameManager.Settings.MinStone + (dayIndex * gameManager.Settings.StoneIncreasePerDay);
        int currentMaxStone = gameManager.Settings.MaxStone + (dayIndex * gameManager.Settings.StoneIncreasePerDay);

       
        gameManager.RequiredFood = Random.Range(currentMinFood, currentMaxFood + 1);
        gameManager.RequiredWood = Random.Range(currentMinWood, currentMaxWood + 1);
        gameManager.RequiredStone = Random.Range(currentMinStone, currentMaxStone + 1);


        gameManager.CollectedFood = 0;
        gameManager.CollectedWood = 0;
        gameManager.CollectedStone = 0;

        gameManager.ResourceSpawner.RespawnAllResourcesForNewDay();
        _player.transform.position = gameManager.House.transform.position;
      
    }

    public void HandleDayEnd()
    {
        if (gameManager.CollectedFood >= gameManager.RequiredFood &&
            gameManager.CollectedWood >= gameManager.RequiredWood &&
            gameManager.CollectedStone >= gameManager.RequiredStone)
        {
            Debug.Log($"Congratulations! You have survived Day {gameManager.CurrentDay}.");

            if (gameManager.CurrentDay >= gameManager.TotalDayCount)
            {
                Debug.Log("YOU WIN! All days completed.");
                gameManager.OnGameWin.Invoke();
                this.enabled = false;
            }
        }
        else
        {
            Debug.Log("Game Over! You failed to collect the required Resources.");
            gameManager.OnGameOver.Invoke();
            this.enabled = false;

        }
    }

    private void OnDestroy()
    {
        if (gameManager != null)
        {
            gameManager.OnNewDayStart.RemoveListener(HandleNewDayStart);
            gameManager.OnDayEnd.RemoveListener(HandleDayEnd);
        }
    }
}