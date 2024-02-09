using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public UnityEvent OnRoundStart, OnRoundEnd;

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
}
