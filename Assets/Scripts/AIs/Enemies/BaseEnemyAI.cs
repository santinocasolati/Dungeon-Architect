using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyAI : SimpleChaseAI
{
    protected override void Start()
    {
        base.Start();

        finalTarget = GameManager.instance.BossInstance;
        enemiesLayer = 1 << LayerMask.NameToLayer("Troops");
    }
}
