using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsManager : MonoBehaviour
{
    public static CoinsManager instance;

    public int startingCoins = 5;

    private int currentCoins;

    public Action<string> coinsModified;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }

        instance = this;
    }

    private void Start()
    {
        ResetManager();
    }

    public void ResetManager()
    {
        SetCoins(startingCoins);
    }

    public void SetCoins(int coins)
    {
        currentCoins = coins;
        coinsModified?.Invoke(currentCoins.ToString());
    }

    public int GetCoins() => currentCoins;

    public void AddCoins(int coins)
    {
        currentCoins += coins;
        coinsModified?.Invoke(currentCoins.ToString());
    }

    public bool RemoveCoins(int coins)
    {
        if (currentCoins - coins < 0)
        {
            return false;
        }

        currentCoins -= coins;
        coinsModified?.Invoke(currentCoins.ToString());
        return true;
    }

    public void PayWithNegatives(int coins)
    {
        currentCoins -= coins;
        coinsModified?.Invoke(currentCoins.ToString());
    }
}
