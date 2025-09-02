using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [Header("Game Dependencies")]
    [SerializeField] private GameSettings settings;
    [SerializeField] private GameObject house;
    [SerializeField] private ResourceSpawner resourceSpawner;
    [HideInInspector] public bool IsGameOver = false;

    [Header("Time Settings")]
    [SerializeField] private int totalDayCount = 10;
    [SerializeField] private float dayDuration = 120f;

    [Header("Game Events")]
    public UnityEvent OnGameWin;
    public UnityEvent OnGameOver;
    public UnityEvent OnNewDayStart;
    public UnityEvent OnDayEnd;
    private TimeManager timeManager;
    public static GameManager Instance;

    [HideInInspector]public int CurrentDay;
    public float TimeLeftInDay { get; set; }
    public int TotalDayCount => totalDayCount;
    public float DayDuration => dayDuration;


    public int RequiredFood { get; set; }
    public int RequiredWood { get; set; }
    public int RequiredStone { get; set; }
    
    public int CollectedFood;
    public int CollectedWood;
    public int CollectedStone;

   


    public GameSettings Settings => settings;
    public GameObject House => house;
    public ResourceSpawner ResourceSpawner => resourceSpawner;

    private void Awake()
    {
        Instance = this;
        timeManager = GetComponent<TimeManager>();
    }

    private void Start()
    {
        timeManager.StartGameClock();
    }
}