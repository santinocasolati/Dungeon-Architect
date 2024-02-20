using UnityEngine;

public class RoomInit : MonoBehaviour
{
    private void OnEnable()
    {
        AstarPath.active.Scan();
    }

    private void OnDisable()
    {
        if (AstarPath.active == null) return;
        AstarPath.active.Scan();
    }
}
