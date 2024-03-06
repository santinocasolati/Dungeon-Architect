using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class RoundManager : MonoBehaviour
{
    public static RoundManager instance;

    public int initialEnemyAmount = 1;
    public int enemyAmountIncrement = 1;
    public int roundsToIncrement = 5;
    public int baseCoinsPerRound = 100;
    public int xpPerRound = 2;
    public int coinsToIncrement = 10;

    public UnityEvent onRoundLost;

    public List<TroopStore> placedTroops = new();

    [SerializeField] private int currentRound = 1;
    private int currentEnemyAmount;
    private int currentCoinsAmount;

    private bool roundStarted = false;

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
        roundStarted = false;
        currentRound = 1;
        currentEnemyAmount = initialEnemyAmount;
        currentCoinsAmount = baseCoinsPerRound;
    }

    public void StartRound()
    {
        if (roundStarted) return;
        roundStarted = true;
        SetOriginalPositions();
        SpawnManager.instance.SpawnEnemies(currentEnemyAmount, GameManager.instance.Floors);
    }

    public void EndRound(bool win, int gainedXp)
    {
        if (!roundStarted) return;

        if (!win)
        {
            onRoundLost?.Invoke();
        }

        CoinsManager.instance.AddCoins(currentCoinsAmount);
        currentRound++;

        if (currentRound % roundsToIncrement == 0)
        {
            currentEnemyAmount += enemyAmountIncrement;
            currentCoinsAmount += coinsToIncrement;
        }

        ResetOriginalPositions();
        SpawnManager.instance.DespawnEnemies();
        GameManager.instance.EndRound();
        DungeonLevelManager.instance.AddXp(xpPerRound + gainedXp);
        roundStarted = false;
    }

    private void SetOriginalPositions()
    {
        placedTroops.ForEach(t =>
        {
            SimpleChaseAI ai = t.troopInstance.GetComponent<SimpleChaseAI>();

            if (ai == null)
            {
                ai = t.troopInstance.GetComponentInChildren<SimpleChaseAI>();
            }

            PositionReset pr = ai.gameObject.GetOrAddComponent<PositionReset>();

            pr.originalPos = ai.gameObject.transform.position;
            pr.parentOriginalPos = ai.transform.parent.position;
            pr.originalRot = ai.gameObject.transform.rotation;
            ai.ResetPath();
        });
    }

    private void ResetOriginalPositions()
    {
        placedTroops.ForEach(t =>
        {
            SimpleChaseAI ai = t.troopInstance.GetComponent<SimpleChaseAI>();

            if (ai == null)
            {
                ai = t.troopInstance.GetComponentInChildren<SimpleChaseAI>();
            }

            ai.StopCurrentPath();

            HealthHandler hh = t.troopInstance.GetComponent<HealthHandler>();

            if (hh == null)
            {
                hh = t.troopInstance.GetComponentInChildren<HealthHandler>();
            }

            if (hh != null)
            {
                hh.ResetHealth();
            }
        });
    }
}
