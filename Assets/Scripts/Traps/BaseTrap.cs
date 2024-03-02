using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTrap : MonoBehaviour
{
    public float trapDamage = 1;
    public int precisionRate = 75;
    public float attackCooldown = 2f;
    public LayerMask enemiesLayer;

    private bool canAttack = true;

    private void OnTriggerStay(Collider other)
    {
        if (!canAttack) return;
        if (!((enemiesLayer & (1 << other.gameObject.layer)) != 0)) return;

        Attack(other);
    }

    protected virtual void Attack(Collider other)
    {
        HealthHandler hh = other.gameObject.GetComponent<HealthHandler>();

        if (hh == null)
        {
            hh = other.gameObject.GetComponentInParent<HealthHandler>();
        }

        if (hh == null)
        {
            hh = other.gameObject.GetComponentInChildren<HealthHandler>();
        }

        if (hh != null)
        {
            if (IsAttackSuccessfull())
            {
                hh.TakeDamage(trapDamage);
            }

            canAttack = false;
            Invoke(nameof(ResetAttackCooldown), attackCooldown);
        }
    }

    private bool IsAttackSuccessfull()
    {
        int randomValue = Random.Range(0, 101);

        return randomValue <= precisionRate;
    }

    private void ResetAttackCooldown()
    {
        canAttack = true;
    }
}
