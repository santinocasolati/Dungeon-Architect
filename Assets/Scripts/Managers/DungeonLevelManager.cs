using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonLevelManager : MonoBehaviour
{
    public static DungeonLevelManager _instance;

    [SerializeField] private int maxLevel = 10;
    [SerializeField] private int xpPrice = 4;
    [SerializeField] private int xpBuyAmount = 4;
    [SerializeField] private List<int> xpPerLevelUp;
    [SerializeField] private List<GameObject> roomsPerLevel;

    public Action<int> levelModified;
    public Action<int, int> xpModified;

    private int currentLevel = 1;
    private int currentXp = 0;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(_instance);
        }

        _instance = this;

        currentLevel = 1;
        currentXp = 0;
        levelModified?.Invoke(currentLevel);
        xpModified?.Invoke(xpPerLevelUp[currentLevel - 1], currentXp);
    }

    public void BuyXp()
    {
        if (!CoinsManager.RemoveCoins(xpPrice)) return;

        AddXp(xpBuyAmount);
    }

    public static void AddXp(int xp)
    {
        _instance.currentXp += xp;

        if (_instance.currentXp >= _instance.xpPerLevelUp[_instance.currentLevel - 1])
        {
            _instance.currentXp -= _instance.xpPerLevelUp[_instance.currentLevel - 1];
            _instance.LevelUp();
        }

        _instance.xpModified?.Invoke(_instance.xpPerLevelUp[_instance.currentLevel - 1], _instance.currentXp);
    }

    public void LevelUp()
    {
        if (currentLevel == maxLevel) return;
        currentLevel++;

        levelModified?.Invoke(currentLevel);
        
        if (currentLevel - 1 < roomsPerLevel.Count && roomsPerLevel[currentLevel - 1] != null)
        {
            roomsPerLevel[currentLevel - 1].SetActive(true);
        }
    }
}
