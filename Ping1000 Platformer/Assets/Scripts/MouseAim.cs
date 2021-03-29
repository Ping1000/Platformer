using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class MouseAim : MonoBehaviour
{
    [Tooltip("The point around which the rotator will rotate.")]
    public Transform rotationPoint;
    [Tooltip("The object to rotate towards the mouse.")]
    public GameObject rotator;
    [Tooltip("A vector offset for the rotate position.")]
    public Vector3 rotatorOffset;

    public Vector3 aimDirection {
        get {
            return (rotator.transform.position + rotatorOffset - rotationPoint.position).normalized;
        }
    }

/*    void FixedUpdate()
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
    }*/

    void OnMouseAim(InputValue value)
    {
        Vector2 rotatorVec = rotator.transform.position - rotationPoint.position;
        Vector2 trackedVec = Camera.main.ScreenToWorldPoint(value.Get<Vector2>())
            - rotationPoint.position;

        float angle = Vector2.SignedAngle(rotatorVec, trackedVec);
        rotator.transform.RotateAround(rotationPoint.position,
            Vector3.forward, angle);
    }
}
