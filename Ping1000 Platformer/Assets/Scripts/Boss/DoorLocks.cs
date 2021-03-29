using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLocks : MonoBehaviour
{
    public GameObject pastDoors;
    public GameObject futureDoors;

    private bool doorsLocked = true;
    public static DoorLocks instance;

    private void Awake() {
        instance = this;
    }

    private void Update() {
        pastDoors.SetActive(doorsLocked && TimeManager.instance.isPast);
        futureDoors.SetActive(doorsLocked && !TimeManager.instance.isPast);
    }

    public static void LockDoors(bool locked) {
        instance.doorsLocked = locked;
    }
}
