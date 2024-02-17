using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyAI : BaseEnemyAI
{
    protected override void Attack()
    {
        base.Attack();

        Debug.Log("Enemy Attack");
    }
}
