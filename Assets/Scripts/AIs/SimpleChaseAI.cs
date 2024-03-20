using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

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
    public float attackDamage = 10f;
    public int precisionRate = 75;

    protected bool attackInCooldown = false;
    protected bool inAttackRange = false;

    protected AIDestinationSetter targetSetter;
    public AIPath pathSetter;
    protected Rigidbody rb;
    protected Animator animator;

    public Transform currentTarget;

    private Vector3 lastPosition;

    public UnityEvent OnAttackMissed;

    private void Awake()
    {
        targetSetter = gameObject.GetComponent<AIDestinationSetter>();
        pathSetter = gameObject.GetComponent<AIPath>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        lastPosition = transform.position;
    }

    protected virtual void Start()
    {   
        pathSetter.endReachedDistance = attackRange;
    }

    void Update()
    {
        animator.SetBool("IsMoving", pathSetter.velocity.magnitude > 0.1f && !pathSetter.isStopped);

        CheckNearEnemies();
        CheckAttack();

        if (pathSetter.isStopped)
        {
            PositionReset pr = GetComponent<PositionReset>();

            if (pr != null)
            {
                transform.position = pr.originalPos;
                transform.rotation = pr.originalRot;
            }
        }

        lastPosition = transform.position;
    }

    private void CheckNearEnemies()
    {
        if (inAttackRange) return;

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
        currentTarget = nearestEnemy;
    }

    private void CheckAttack()
    {
        if (targetSetter.target == null)
        {
            inAttackRange = false;
            return;
        }

        if (Vector3.Distance(targetSetter.target.position, transform.position) < attackRange && !attackInCooldown)
        {
            inAttackRange = true;

            Vector3 direction = targetSetter.target.position - transform.position;
            Vector3 planeDirection = Vector3.ProjectOnPlane(direction, Vector3.up);
            float angle = Vector3.SignedAngle(transform.forward, planeDirection, Vector3.up);
            transform.Rotate(Vector3.up, angle, Space.World);

            Attack();
        } else
        {
            inAttackRange = false;
        }
    }

    protected virtual void Attack()
    {
        animator.ResetTrigger("Attack");

        if (currentTarget != null)
        {
            HealthHandler hh = currentTarget.gameObject.GetComponent<HealthHandler>();

            if (hh == null)
            {
                hh = currentTarget.gameObject.GetComponentInParent<HealthHandler>();
            }

            if (hh == null)
            {
                hh = currentTarget.gameObject.GetComponentInChildren<HealthHandler>();
            }

            if (hh != null)
            {
                attackInCooldown = true;

                animator.SetTrigger("Attack");

                if (IsAttackSuccessfull())
                {
                    hh.TakeDamage(attackDamage);
                } else
                {
                    OnAttackMissed?.Invoke();
                }
                
                Invoke(nameof(ResetAttack), attackCooldown);
            }
        }
    }

    private bool IsAttackSuccessfull()
    {
        int randomValue = UnityEngine.Random.Range(0, 101);

        return randomValue <= precisionRate;
    }

    private void ResetAttack()
    {
        attackInCooldown = false;
    }

    public void StopCurrentPath()
    {
        animator.SetBool("IsMoving", false);
        animator.ResetTrigger("Attack");
        animator.Play("Idle");

        targetSetter.target = transform;
        currentTarget = null;
        pathSetter.isStopped = true;
    }

    public void ResetPath()
    {
        targetSetter.target = null;
        currentTarget = null;
        pathSetter.isStopped = false;
    }
}
