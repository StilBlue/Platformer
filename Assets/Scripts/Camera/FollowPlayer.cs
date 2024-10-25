using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    [SerializeField] Vector3 offset;
    [SerializeField] float smoothTime;

    Vector3 currentVelocity;

    void FixedUpdate()
    {
        Vector3 targetPosition = playerTransform.position + offset;
        Vector3 newLocation = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, smoothTime);

        transform.position = newLocation;
    }
}
