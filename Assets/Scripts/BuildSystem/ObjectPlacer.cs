using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    [SerializeField] private Transform placeContainer;

    private List<GameObject> placedGameObjects = new();

    public int PlaceObject(GameObject prefab, Vector3 objectPos, ObjectType objectType)
    {
        GameObject objectInstance = Instantiate(prefab, placeContainer);
        objectInstance.transform.position = objectPos;
        placedGameObjects.Add(objectInstance);

        HealthHandler healthHandler = objectInstance.GetComponent<HealthHandler>();

        if (healthHandler == null)
        {
            objectInstance.GetComponentInChildren<HealthHandler>();
        }

        if (healthHandler != null)
        {
            TroopStore ts = new TroopStore();
            ts.troopInstance = objectInstance;
            ts.troopIndex = placedGameObjects.Count - 1;

            RoundManager._instance.placedTroops.Add(ts);

            if (objectType == ObjectType.Boss)
            {
                GameManager.BossStatus(true, objectInstance.transform);
            }
            else
            {
                healthHandler.OnDeath.AddListener(() => RemoveObjectAt(ts.troopIndex, false));
            }
        }

        return placedGameObjects.Count - 1;
    }

    public void RemoveObjectAt(int gameObjectIndex, bool isBoss)
    {
        if (placedGameObjects.Count <= gameObjectIndex || placedGameObjects[gameObjectIndex] == null) return;

        if (isBoss)
        {
            GameManager.BossStatus(false, null);
        }

        TroopStore foundTroop = RoundManager._instance.placedTroops.FirstOrDefault(troop => troop.troopIndex == gameObjectIndex);
        if (foundTroop != null)
        {
            RoundManager._instance.placedTroops.Remove(foundTroop);
        }


        Destroy(placedGameObjects[gameObjectIndex]);
    }
}
