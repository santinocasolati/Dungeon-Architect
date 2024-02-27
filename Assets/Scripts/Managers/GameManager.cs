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

    public List<GameObject> floors;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }

        instance = this;

        ResetGame();
    }

    public void ResetGame()
    {
        BossPlaced = false;
        BossInstance = null;

        originalFloors.ForEach(floor =>
        {
            floors.Add(floor);
        });

        RoundManager.instance.ResetManager();
        SpawnManager.instance.ResetManager();
        DungeonHPManager.instance.ResetManager();
        DungeonLevelManager.instance.ResetManager();
        CoinsManager.instance.ResetManager();
        KilledEnemyManager.instance.ResetManager();
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

        KilledEnemyManager.instance.StartCleaning();
    }

    public void RoomPurchase(GameObject floor)
    {
        floors.Add(floor);
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
