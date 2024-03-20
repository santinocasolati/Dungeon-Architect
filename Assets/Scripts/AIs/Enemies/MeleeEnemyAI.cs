using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyAI : MeleeAI
{
    protected override void Start()
    {
        base.Start();

        finalTarget = GameManager._instance.BossInstance;
    }
}
