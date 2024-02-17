using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTroopAI : BaseTroopAI
{
    protected override void Attack()
    {
        base.Attack();

        Debug.Log("Troop Attack");
    }
}
