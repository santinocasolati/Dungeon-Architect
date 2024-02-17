using System;
using System.Collections;
using System.Collections.Generic;
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

        Destroy(placedGameObjects[gameObjectIndex]);
    }
}
