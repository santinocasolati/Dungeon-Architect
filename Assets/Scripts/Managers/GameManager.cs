using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public UnityEvent OnRoundStart, OnRoundEnd;
    public bool bossPlaced { get; private set; }
    public Transform bossInstance { get; private set; }
    public List<GameObject> floors;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }

        instance = this;
    }

    public void StartRound()
    {
        //TODO: a notification to place the boss
        if (!bossPlaced) return;

        OnRoundStart?.Invoke();
    }

    public void EndRound()
    {
        OnRoundEnd?.Invoke();
    }

    public void RoomPurchase(GameObject floor)
    {
        floors.Add(floor);
    }

    public void BossStatus(bool bossPlaced, Transform bossInstance)
    {
        this.bossPlaced = bossPlaced;
        this.bossInstance = bossInstance;
    }

    public void Defeat()
    {
        Debug.Log("Defeat");
    }
}
