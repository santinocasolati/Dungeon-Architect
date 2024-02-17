using UnityEngine;

public class RoomInit : MonoBehaviour
{
    private void OnEnable()
    {
        AstarPath.active.Scan();
    }

    private void OnDisable()
    {
        AstarPath.active.Scan();
    }
}
