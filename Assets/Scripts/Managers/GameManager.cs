using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public UnityEvent OnRoundStart, OnRoundEnd;

    [SerializeField] private NavMeshSurface walkableSurface;

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
}
