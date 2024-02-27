using UnityEngine;

public class RoomInit : MonoBehaviour
{
    [SerializeField] private bool isStartingRoom = false;

    private void OnEnable()
    {
        AstarPath.active.Scan();
        
        if (!isStartingRoom)
        {
            GameManager.instance.RoomPurchase(gameObject);
        }
    }

    private void OnDisable()
    {
        if (AstarPath.active == null) return;
        AstarPath.active.Scan();
    }
}
