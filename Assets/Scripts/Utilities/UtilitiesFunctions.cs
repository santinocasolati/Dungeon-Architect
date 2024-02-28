using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilitiesFunctions : MonoBehaviour
{
    public static UtilitiesFunctions instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ManagerSingleton(GameObject manager)
    {
        manager.transform.parent = null;
        DontDestroyOnLoad(manager);
    }
}
