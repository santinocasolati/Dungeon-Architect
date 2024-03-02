using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class HealthHandler : MonoBehaviour
{
    public float maxHealth = 100;
    public bool isBoss = false;
    public bool isEnemy = false;
    public float currentHealth;

    public UnityEvent OnDeath;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float heal)
    {
        currentHealth += heal;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
    }

    private void Die()
    {
        OnDeath?.Invoke();

        if (!isEnemy)
        {
            if (isBoss)
            {
                RoundManager.instance.EndRound(false);
            } else
            {
                SimpleChaseAI ai = gameObject.GetComponent<SimpleChaseAI>();

                if (ai == null)
                {
                    ai = gameObject.GetComponentInChildren<SimpleChaseAI>();
                }

                PositionReset pr = ai.gameObject.GetComponent<PositionReset>();
                KilledTroopsManager.instance.AddKilled(pr.parentOriginalPos);
                gameObject.SetActive(false);
            }
        }
    }
}
