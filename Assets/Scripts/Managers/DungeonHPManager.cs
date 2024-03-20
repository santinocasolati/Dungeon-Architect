using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonHPManager : MonoBehaviour
{
    public static DungeonHPManager _instance;

    [SerializeField] private int maxHp = 10;

    public Action<int, int> DungeonHPModified;

    private int currentHp;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(_instance);
        }

        _instance = this;

        currentHp = maxHp;
        DungeonHPModified?.Invoke(maxHp, currentHp);
    }

    public void DamageDungeon(int damage)
    {
        currentHp -= damage;

        if (currentHp <= 0)
        {
            currentHp = 0;
            GameManager.Defeat();
        }

        DungeonHPModified?.Invoke(maxHp, currentHp);
    }

    public void HealDungeon(int heal)
    {
        currentHp += heal;

        if (currentHp > maxHp)
        {
            currentHp = maxHp;
        }

        DungeonHPModified?.Invoke(maxHp, currentHp);
    }
}
