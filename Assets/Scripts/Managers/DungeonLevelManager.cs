using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonLevelManager : MonoBehaviour
{
    public static DungeonLevelManager instance;

    [SerializeField] private int maxLevel = 10;
    [SerializeField] private int xpPrice = 4;
    [SerializeField] private int xpBuyAmount = 4;
    [SerializeField] private List<int> xpPerLevelUp;
    [SerializeField] private List<GameObject> roomsPerLevel;

    public Action<int> levelModified;
    public Action<int, int> xpModified;

    private int currentLevel = 1;
    private int currentXp = 0;

    private void OnEnable()
    {
        if (instance != null)
        {
            Destroy(instance);
        }

        instance = this;

        currentLevel = 1;
        currentXp = 0;
        levelModified?.Invoke(currentLevel);
        xpModified?.Invoke(xpPerLevelUp[currentLevel - 1], currentXp);
    }

    public void BuyXp()
    {
        if (!CoinsManager.instance.RemoveCoins(xpPrice)) return;

        AddXp(xpBuyAmount);
    }

    public void AddXp(int xp)
    {
        currentXp += xp;

        if (currentXp >= xpPerLevelUp[currentLevel - 1])
        {
            currentXp -= xpPerLevelUp[currentLevel - 1];
            LevelUp();
        }

        xpModified?.Invoke(xpPerLevelUp[currentLevel - 1], currentXp);
    }

    public void LevelUp()
    {
        if (currentLevel == maxLevel) return;
        currentLevel++;

        levelModified?.Invoke(currentLevel);
        
        if (currentLevel - 1 < roomsPerLevel.Count)
        {
            roomsPerLevel[currentLevel - 1].SetActive(true);
        }
    }
}
