using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AIPath))]
[RequireComponent(typeof(AIDestinationSetter))]
public class SimpleChaseAI : MonoBehaviour
{
    public Transform finalTarget;
    public LayerMask enemiesLayer;
    public float detectionRange = 5f;
    public float attackRange = 2.5f;
    public float attackCooldown = 2f;
    public float avoidanceRadius = 1.0f;

    protected bool attackInCooldown = false;

    protected AIDestinationSetter targetSetter;
    protected AIPath pathSetter;
    protected Rigidbody rb;

    private void Awake()
    {
        targetSetter = gameObject.GetComponent<AIDestinationSetter>();
        pathSetter = gameObject.GetComponent<AIPath>();
        rb = GetComponent<Rigidbody>();
    }

    protected virtual void Start()
    {   
        pathSetter.endReachedDistance = attackRange;
    }

    void Update()
    {
        CheckNearEnemies();
        CheckAttack();
    }

    private void CheckNearEnemies()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange, enemiesLayer);

        Transform nearestEnemy = finalTarget;

        float nearestDistance = Mathf.Infinity;

        foreach (Collider collider in colliders)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, collider.transform.position);
            if (distanceToEnemy < nearestDistance)
            {
                nearestEnemy = collider.gameObject.transform;
                nearestDistance = distanceToEnemy;
            }
        }

        targetSetter.target = nearestEnemy;
    }

    private void CheckAttack()
    {
        if (targetSetter.target == null) return;

        if (Vector3.Distance(targetSetter.target.position, transform.position) < attackRange && !attackInCooldown)
        {
            Attack();
        }
    }

    protected virtual void Attack()
    {
        attackInCooldown = true;

        Invoke(nameof(ResetAttack), attackCooldown);
    }

    private void ResetAttack()
    {
        attackInCooldown = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
