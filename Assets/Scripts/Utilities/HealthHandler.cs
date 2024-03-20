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

        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.material.color = Color.red;
        }

        Invoke(nameof(ResetColor), 0.15f);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void ResetColor()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.material.color = Color.white;
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
                RoundManager.EndRound(false, SpawnManager._instance.xpGained);
            } else
            {
                SimpleChaseAI ai = gameObject.GetComponent<SimpleChaseAI>();

                if (ai == null)
                {
                    ai = gameObject.GetComponentInChildren<SimpleChaseAI>();
                }

                PositionReset pr = ai.gameObject.GetComponent<PositionReset>();
                KilledTroopsManager.AddKilled(pr.parentOriginalPos);
                gameObject.SetActive(false);
            }
        }
    }
}
