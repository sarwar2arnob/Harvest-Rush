using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

public class TimeManager : MonoBehaviour
{
    private GameManager gameManager;

    private CancellationTokenSource _cancellationTokenSource;

    private void Start()
    {
        gameManager = GameManager.Instance;
        _cancellationTokenSource = new CancellationTokenSource();
    }

    private void OnDestroy()
    {

        _cancellationTokenSource.Cancel();
        _cancellationTokenSource.Dispose();
    }

    public void StartGameClock()
    {
        DayCycleAsync(_cancellationTokenSource.Token).Forget();
    }

    private async UniTask DayCycleAsync(CancellationToken token)
    {
        try
        {
            while (gameManager.CurrentDay < gameManager.TotalDayCount)
            {
                gameManager.CurrentDay++;
                gameManager.TimeLeftInDay = gameManager.DayDuration;
                gameManager.OnNewDayStart.Invoke();

                while (gameManager.TimeLeftInDay > 0)
                {
                    if (gameManager.IsGameOver) return;
                    token.ThrowIfCancellationRequested();

                    gameManager.TimeLeftInDay -= Time.deltaTime;
                    await UniTask.Yield(PlayerLoopTiming.Update, token);
                }

                gameManager.TimeLeftInDay = 0;
                gameManager.OnDayEnd.Invoke();

                if (!gameManager.IsGameOver)
                {
                    await UniTask.Delay(System.TimeSpan.FromSeconds(2f), cancellationToken: token);
                }
            }
        }
        catch (System.OperationCanceledException)
        {
            Debug.Log("Day cycle task was cancelled.");
        }
    }
}