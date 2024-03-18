using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeTroopAI : MeleeAI
{
    protected override void Start()
    {
        base.Start();

        finalTarget = null;
    }
}
