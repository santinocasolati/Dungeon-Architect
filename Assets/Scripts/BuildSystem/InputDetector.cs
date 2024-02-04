using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputDetector : MonoBehaviour
{
    [SerializeField] LayerMask placementLayermask;
    [SerializeField] private Camera cam;

    private Vector3 lastPosition;

    public event Action OnMousePressed, OnCancel;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnMousePressed?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnCancel?.Invoke();
        }
    }

    public bool IsPointerOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    public Vector3 GetSelectedMapPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = cam.nearClipPlane;
        Ray ray = cam.ScreenPointToRay(mousePos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, placementLayermask))
        {
            lastPosition = hit.point;
        }

        return lastPosition;
    }
}
