﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseAim : MonoBehaviour
{
    [Tooltip("The point around which the rotator will rotate.")]
    public Transform rotationPoint;
    [Tooltip("The object to rotate towards the mouse.")]
    public GameObject rotator;

    public Vector3 aimDirection {
        get {
            return (rotator.transform.position - rotationPoint.position).normalized;
        }
    }

    void FixedUpdate()
    {
        TrackObject();
    }

    void TrackObject() {
        Vector2 rotatorVec = rotator.transform.position - rotationPoint.position;
        Vector2 trackedVec = Camera.main.ScreenToWorldPoint(Input.mousePosition)
            - rotationPoint.position;

        float angle = Vector2.SignedAngle(rotatorVec, trackedVec);
        rotator.transform.RotateAround(rotationPoint.position, 
            Vector3.forward, angle);
    }
}