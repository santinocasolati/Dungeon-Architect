using UnityEngine;

public class RoomInit : MonoBehaviour
{
    private void Start()
    {
        AstarPath.active.Scan();

        GameManager.RoomPurchase(gameObject);
    }

    private void OnDisable()
    {
        if (AstarPath.active == null) return;
        AstarPath.active.Scan();
    }
}
