using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsManager : MonoBehaviour
{
    public static CoinsManager _instance;

    public int startingCoins = 5;

    private int currentCoins;

    public Action<string> coinsModified;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(_instance);
        }

        _instance = this;

        SetCoins(startingCoins);
    }

    public void SetCoins(int coins)
    {
        currentCoins = coins;
        coinsModified?.Invoke(currentCoins.ToString());
    }

    public static int GetCoins() => _instance.currentCoins;

    public static void AddCoins(int coins)
    {
        _instance.currentCoins += coins;
        _instance.coinsModified?.Invoke(_instance.currentCoins.ToString());
    }

    public static bool RemoveCoins(int coins)
    {
        if (_instance.currentCoins - coins < 0)
        {
            return false;
        }

        _instance.currentCoins -= coins;
        _instance.coinsModified?.Invoke(_instance.currentCoins.ToString());
        return true;
    }

    public static void PayWithNegatives(int coins)
    {
        _instance.currentCoins -= coins;
        _instance.coinsModified?.Invoke(_instance.currentCoins.ToString());
    }
}
