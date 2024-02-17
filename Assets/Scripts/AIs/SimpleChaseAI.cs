using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AIPath))]
[RequireComponent(typeof(AIDestinationSetter))]
public class SimpleChaseAI : MonoBehaviour
{
    private AIDestinationSetter targetSetter;

    private void Start()
    {
        targetSetter = gameObject.GetComponent<AIDestinationSetter>();

        targetSetter.target = GameManager.instance.bossInstance;
    }
}
