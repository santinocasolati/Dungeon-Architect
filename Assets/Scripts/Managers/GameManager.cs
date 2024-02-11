using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public UnityEvent OnRoundStart, OnRoundEnd;
    public bool bossPlaced { get; private set; }
    public GameObject bossInstance { get; private set; }

    [SerializeField] private NavMeshSurface walkableSurface;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }

        instance = this;
    }

    private void OnEnable()
    {
        OnRoundStart.AddListener(RoundStart);
        OnRoundStart.AddListener(RoundEnd);
    }

    private void RoundStart()
    {

    }

    private void RoundEnd()
    {

    }

    public void RoomPurchase()
    {
        walkableSurface.BuildNavMesh();
    }

    public void BossStatus(bool bossPlaced, GameObject bossInstance)
    {
        this.bossPlaced = bossPlaced;
        this.bossInstance = bossInstance;
    }
}
