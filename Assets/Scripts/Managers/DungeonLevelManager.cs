using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonLevelManager : MonoBehaviour
{
    public static DungeonLevelManager instance;

    [SerializeField] private int maxLevel = 10;
    [SerializeField] private List<GameObject> roomsPerLevel;

    public Action<int> levelModified;

    private int currentLevel = 1;

    private void OnEnable()
    {
        if (instance != null)
        {
            Destroy(instance);
        }

        instance = this;
        UtilitiesFunctions.instance.ManagerSingleton(gameObject);
    }

    public void ResetManager()
    {
        currentLevel = 1;
        levelModified?.Invoke(currentLevel);
    }

    public void LevelUp()
    {
        if (currentLevel == maxLevel) return;
        currentLevel++;

        levelModified?.Invoke(currentLevel);
        roomsPerLevel[currentLevel - 1].SetActive(true);
    }
}
