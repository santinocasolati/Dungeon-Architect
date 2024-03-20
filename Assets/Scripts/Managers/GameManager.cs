using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance;

    public UnityEvent OnRoundStart, OnRoundEnd;
    public bool BossPlaced { get; private set; }
    public Transform BossInstance { get; private set; }

    public List<GameObject> Floors { get; private set; }

    private void Start()
    {
        if (_instance != null)
        {
            Destroy(_instance);
        }

        _instance = this;

        Floors = new();

        BossPlaced = false;
        BossInstance = null;
    }

    public void StartRound()
    {
        if (!BossPlaced) return;

        OnRoundStart?.Invoke();
    }

    public static void EndRound()
    {
        _instance.OnRoundEnd?.Invoke();

        KilledTroopsManager.StartCleaning();
    }

    public static void RoomPurchase(GameObject floor)
    {
        _instance.Floors.Add(floor);
    }

    public static void BossStatus(bool bossPlaced, Transform bossInstance)
    {
        _instance.BossPlaced = bossPlaced;
        _instance.BossInstance = bossInstance;
    }

    public static void Defeat()
    {
        Debug.Log("Defeat");
    }
}
