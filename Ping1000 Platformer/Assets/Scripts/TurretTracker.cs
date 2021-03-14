using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretTracker : MonoBehaviour
{
    public GameObject trackedObject; // maybe we can upgrade to a list or by tag
    public float visibleRadius; // maybe we can change to be a sector
    public float rotationSpeed;
    [Tooltip("The point around which the rotator will rotate.")]
    public Transform rotationPoint;
    [Tooltip("The object to rotate towards the player.")]
    public GameObject rotator;
    [Tooltip("Optional gun for shooting at player.")]
    public GunController gun;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float distance = Vector2.Distance(transform.position,
            trackedObject.transform.position);
        if (distance <= visibleRadius) {
            TrackObject();
            if (gun != null) {
                // shoot
                gun.canShoot = true;
            }
        } else {
            if (gun != null) {
                // stop shooting
                gun.canShoot = false;
            }
        }
    }

    void TrackObject() {
        Vector2 rotatorVec = rotator.transform.position - rotationPoint.position;
        Vector2 trackedVec = trackedObject.transform.position - rotationPoint.position;

        float angle = Vector2.SignedAngle(rotatorVec, trackedVec);
        rotator.transform.RotateAround(rotationPoint.position, Vector3.forward, 
            angle * Time.fixedDeltaTime * rotationSpeed);
        // rotator.transform.RotateAround(rotationPoint.transform.position, Vector3.forward, 20 * Time.deltaTime);
    }

    public void OnDrawGizmosSelected() {
        Gizmos.DrawWireSphere(transform.position, visibleRadius);
    }
}
