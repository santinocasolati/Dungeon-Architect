using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonHPManager : MonoBehaviour
{
    public static DungeonHPManager instance;

    [SerializeField] private int maxHp = 10;

    public Action<int, int> DungeonHPModified;

    private int currentHp;

    private void OnEnable()
    {
        if (instance != null)
        {
            Destroy(instance);
        }

        instance = this;

        currentHp = maxHp;
        DungeonHPModified?.Invoke(maxHp, currentHp);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            DamageDungeon(5);
        }
    }

    public void DamageDungeon(int damage)
    {
        currentHp -= damage;

        if (currentHp <= 0)
        {
            currentHp = 0;
            GameManager.instance.Defeat();
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
