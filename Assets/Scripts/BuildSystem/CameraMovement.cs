using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField, Min(0.1f)] private float speed = 1f;
    [SerializeField] private float rotationSpeed = 15;
    [SerializeField] private float movementTime = 0.1f;
    [SerializeField] private Vector3 zoomAmount, zoomLimitClose, zoomLimitFar;
    [SerializeField] private CinemachineVirtualCamera cameraReference;
    [SerializeField] private int constraintXMax = 5, constraintXMin = -5, constraintZMax = 5, constraintZMin = -5;

    private CinemachineTransposer cameraTransposer;
    private Vector3 newZoom;
    private Quaternion targetRotation;
    private Vector2 input;

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Vector3 originalFollowOffset;
    private bool isResetting = false;

    private void Start()
    {
        cameraTransposer = cameraReference.GetCinemachineComponent<CinemachineTransposer>();
        targetRotation = transform.rotation;
        newZoom = cameraTransposer.m_FollowOffset;

        originalPosition = transform.position;
        originalRotation = transform.rotation;
        originalFollowOffset = cameraTransposer.m_FollowOffset;
    }

    void Update()
    {
        if (isResetting) return;
        input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        int rotationDirection = 0;
        if (Input.GetKey(KeyCode.Q))
            rotationDirection = -1;
        if (Input.GetKey(KeyCode.E))
            rotationDirection = 1;
        targetRotation = transform.rotation * Quaternion.Euler(Vector3.up * rotationDirection * rotationSpeed);

        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetCamera();
            return;
        }

        if (Mathf.Approximately(Input.mouseScrollDelta.y, 0) == false)
        {
            int zoomDirection = Input.mouseScrollDelta.y > 0 ? 1 : -1;
            newZoom += zoomAmount * zoomDirection;
            newZoom = ClampVector(newZoom, zoomLimitClose, zoomLimitFar);
        }

        transform.position += (transform.forward * input.y + transform.right * input.x) * speed * Time.deltaTime;

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime / movementTime);
        cameraTransposer.m_FollowOffset = Vector3.Lerp(cameraTransposer.m_FollowOffset, newZoom, Time.deltaTime / movementTime);
    }

    private void ResetCamera()
    {
        isResetting = true;
        StartCoroutine(ResetTransition());
    }

    private IEnumerator ResetTransition()
    {
        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;
        Vector3 startFollowOffset = cameraTransposer.m_FollowOffset;

        float smoothDuration = 1f;
        float elapsedTime = 0f;

        while (elapsedTime < smoothDuration)
        {
            transform.position = Vector3.Lerp(startPosition, originalPosition, elapsedTime / smoothDuration);
            targetRotation = Quaternion.Slerp(startRotation, originalRotation, elapsedTime / smoothDuration);
            transform.rotation = Quaternion.Slerp(startRotation, originalRotation, elapsedTime / smoothDuration);
            cameraTransposer.m_FollowOffset = Vector3.Lerp(startFollowOffset, originalFollowOffset, elapsedTime / smoothDuration);
            newZoom = Vector3.Lerp(startFollowOffset, originalFollowOffset, elapsedTime / smoothDuration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPosition;
        transform.rotation = originalRotation;
        targetRotation = originalRotation;
        cameraTransposer.m_FollowOffset = originalFollowOffset;
        newZoom = originalFollowOffset;
        isResetting = false;
    }

    private Vector3 ClampVector(Vector3 newZoom, Vector3 zoomLimitClose, Vector3 zoomLimitFar)
    {
        newZoom.x = Mathf.Clamp(newZoom.x, zoomLimitClose.x, zoomLimitFar.x);
        newZoom.y = Mathf.Clamp(newZoom.y, zoomLimitClose.y, zoomLimitFar.y);
        newZoom.z = Mathf.Clamp(newZoom.z, zoomLimitClose.z, zoomLimitFar.z);
        return newZoom;
    }
}
