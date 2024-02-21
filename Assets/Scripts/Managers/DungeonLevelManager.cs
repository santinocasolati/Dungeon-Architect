using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonLevelManager : MonoBehaviour
{
    public static DungeonLevelManager instance;

    [SerializeField] private int maxLevel = 10;
    private int currentLevel = 1;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }

        instance = this;
    }

    public void LevelUp()
    {
        if (currentLevel == maxLevel) return;
        currentLevel++;

        // TODO: Allow to buy one expansion room
    }
}
