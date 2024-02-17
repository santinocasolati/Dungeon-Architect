using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTroopAI : SimpleChaseAI
{
    protected override void Start()
    {
        base.Start();

        finalTarget = null;
        enemiesLayer = 1 << LayerMask.NameToLayer("Enemy");
    }
}
