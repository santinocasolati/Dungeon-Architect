using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsManager : MonoBehaviour
{
    public static CoinsManager instance;

    [SerializeField] private int maxCoins;
    public int startingCoins = 5;

    public int currentCoins;

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
        SetCoins(startingCoins);
    }

    public void SetCoins(int coins)
    {
        currentCoins = coins;
    }

    public void AddCoins(int coins)
    {
        currentCoins += coins;

        if (currentCoins > maxCoins)
        {
            currentCoins = maxCoins;
        }
    }

    public bool RemoveCoins(int coins)
    {
        if (currentCoins - coins < 0)
        {
            // TODO: Error Feedback
            return false;
        }

        currentCoins -= coins;
        return true;
    }
}
