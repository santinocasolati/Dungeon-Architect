using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTrap : BaseTrap
{
    protected override void Attack(Collider other)
    {
        base.Attack(other);

        //Do animation and stuff
    }
}
