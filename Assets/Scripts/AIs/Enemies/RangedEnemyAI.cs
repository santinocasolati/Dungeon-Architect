using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyAI : RangedAI
{
    protected override void Start()
    {
        base.Start();

        finalTarget = GameManager._instance.BossInstance;
    }
}
