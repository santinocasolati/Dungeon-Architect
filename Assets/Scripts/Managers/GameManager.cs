using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public UnityEvent OnRoundStart, OnRoundEnd;
    public bool BossPlaced { get; private set; }
    public Transform BossInstance { get; private set; }
    [SerializeField] private List<GameObject> originalFloors;

    public List<GameObject> Floors { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }

        instance = this;
        Floors = new();
    }

    private void Start()
    {
        ResetGame();
    }

    public void ResetGame()
    {
        BossPlaced = false;
        BossInstance = null;

        originalFloors.ForEach(floor =>
        {
            Floors.Add(floor);
        });

        RoundManager.instance.ResetManager();
        SpawnManager.instance.ResetManager();
        DungeonHPManager.instance.ResetManager();
        DungeonLevelManager.instance.ResetManager();
        CoinsManager.instance.ResetManager();
        KilledTroopsManager.instance.ResetManager();
    }

    public void StartRound()
    {
        //TODO: a notification to place the boss
        if (!BossPlaced) return;

        OnRoundStart?.Invoke();
    }

    public void EndRound()
    {
        OnRoundEnd?.Invoke();

        KilledTroopsManager.instance.StartCleaning();
    }

    public void RoomPurchase(GameObject floor)
    {
        Floors.Add(floor);
    }

    public void BossStatus(bool bossPlaced, Transform bossInstance)
    {
        this.BossPlaced = bossPlaced;
        this.BossInstance = bossInstance;
    }

    public void Defeat()
    {
        ResetGame();
        Debug.Log("Defeat");
    }
}
