using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonHPManager : MonoBehaviour
{
    public static DungeonHPManager instance;

    [SerializeField] private int maxHp = 10;

    private int currentHp;

    private void OnEnable()
    {
        if (instance != null)
        {
            Destroy(instance);
        }

        instance = this;
        UtilitiesFunctions.instance.ManagerSingleton(gameObject);

        ResetManager();
    }

    public void ResetManager()
    {
        currentHp = maxHp;
    }

    public void DamageDungeon(int damage)
    {
        currentHp -= damage;

        if (currentHp <= 0)
        {
            GameManager.instance.Defeat();
        }
    }

    public void HealDungeon(int heal)
    {
        currentHp += heal;

        if (currentHp > maxHp)
        {
            currentHp = maxHp;
        }
    }
}
