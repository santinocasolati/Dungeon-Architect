using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedTroopAI : RangedAI
{
    protected override void Start()
    {
        base.Start();

        finalTarget = null;
    }
}
