using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonLevelManager : MonoBehaviour
{
    public static DungeonLevelManager instance;

    [SerializeField] private int maxLevel = 10;
    [SerializeField] private List<GameObject> roomsPerLevel;

    private int currentLevel = 1;

    private void Awake()
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
    }

    public void LevelUp()
    {
        if (currentLevel == maxLevel) return;
        currentLevel++;

        roomsPerLevel[currentLevel - 1].SetActive(true);
    }
}
