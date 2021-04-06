using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class MouseAim : MonoBehaviour
{
    private EricCharacterMovement player;

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

    private void Start() {
        player = GetComponent<EricCharacterMovement>();
    }

    void Update()
    {
        Vector2 rotatorVec = rotator.transform.position - rotationPoint.position;
        // Vector2 rotatorVec = aimDirection;
        Vector2 trackedVec = Camera.main.ScreenToWorldPoint(Input.mousePosition)
            - rotationPoint.position;

        float angle = Vector2.SignedAngle(rotatorVec, trackedVec);
        rotator.transform.RotateAround(rotationPoint.position,
            Vector3.forward, angle);

        if (player != null)
            FlipPlayerToCursor(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

    void FlipPlayerToCursor(Vector2 cursorWorldPos) {
        if (cursorWorldPos.x > rotationPoint.position.x && !player.facingRight)
            player.Flip();
        if (cursorWorldPos.x < rotationPoint.position.x && player.facingRight)
            player.Flip();
    }
}
