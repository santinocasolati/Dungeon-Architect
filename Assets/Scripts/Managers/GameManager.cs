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

    public List<GameObject> Floors { get; private set; }

    private void Start()
    {
        if (instance != null)
        {
            Destroy(instance);
        }

        instance = this;

        Floors = new();

        BossPlaced = false;
        BossInstance = null;
    }

    public void StartRound()
    {
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
        Debug.Log("Defeat");
    }
}
