using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    [SerializeField] private Transform placeContainer;

    private List<GameObject> placedGameObjects = new();

    public int PlaceObject(GameObject prefab, Vector3 objectPos)
    {
        GameObject objectInstance = Instantiate(prefab, placeContainer);
        objectInstance.transform.position = objectPos;
        placedGameObjects.Add(objectInstance);
        return placedGameObjects.Count - 1;
    }

    public void RemoveObjectAt(int gameObjectIndex)
    {
        if (placedGameObjects.Count <= gameObjectIndex || placedGameObjects[gameObjectIndex] == null) return;

        Destroy(placedGameObjects[gameObjectIndex]);
    }
}
