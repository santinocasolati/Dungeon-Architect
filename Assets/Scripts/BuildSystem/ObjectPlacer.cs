using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    [SerializeField] private Transform placeContainer;

    private List<GameObject> placedGameObjects = new();

    public int PlaceObject(GameObject prefab, Vector3 objectPos, bool isBoss)
    {
        GameObject objectInstance = Instantiate(prefab, placeContainer);
        objectInstance.transform.position = objectPos;
        placedGameObjects.Add(objectInstance);

        if (isBoss)
        {
            GameManager.instance.BossStatus(true, objectInstance.transform);
        } else
        {
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

                RoundManager.instance.placedTroops.Add(ts);

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
            GameManager.instance.BossStatus(false, null);
        }

        TroopStore foundTroop = RoundManager.instance.placedTroops.FirstOrDefault(troop => troop.troopIndex == gameObjectIndex);
        if (foundTroop != null)
        {
            RoundManager.instance.placedTroops.Remove(foundTroop);
        }


        Destroy(placedGameObjects[gameObjectIndex]);
    }
}
