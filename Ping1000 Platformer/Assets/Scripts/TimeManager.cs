using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public GameObject pastParent;
    public GameObject pastTilemap;
    public GameObject presentParent;
    public GameObject presentTilemap;

    public static TimeManager instance;
    /// <summary>
    /// The parent Transform for the active time period. Useful
    /// when instantiating
    /// </summary>
    public static Transform activeTimeParent { get {
            if (instance.isPast)
                return instance.pastParent.transform;
            return instance.presentParent.transform;
        } }

    public bool isPast;

    private void Awake() {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        SetTime(isPast);
    }

    /// <summary>
    /// Swaps the global time by enabling/disabling the parent
    /// past/future GameObjects and Tilemaps. Mostly used when
    /// the player's time needs to be changed.
    /// </summary>
    public static void SwapTime() {
        instance.isPast = !instance.isPast;
        SetTime(instance.isPast);
    }

    /// <summary>
    /// Sends a GameObject into the other time.
    /// </summary>
    /// <param name="swappedObj">The object to send</param>
    public static void SwapTime(GameObject swappedObj) {
        if (instance.isPast) {
            swappedObj.transform.parent = instance.presentParent.transform;
        } else {
            swappedObj.transform.parent = instance.pastParent.transform;
        }
    }

    /// <summary>
    /// Sets the global time to isPast by enabling/disabling the parent
    /// past/future GameObjects and Tilemaps
    /// </summary>
    /// <param name="isPast"></param>
    public static void SetTime(bool isPast) {
        // Despawn all bullets (keep?)
        //foreach (Bullet bullet in FindObjectsOfType<Bullet>()) {
        //    Destroy(bullet.gameObject);
        //}

        instance.pastParent.SetActive(isPast);
        instance.pastTilemap.SetActive(isPast);
        instance.presentParent.SetActive(!isPast);
        instance.presentTilemap.SetActive(!isPast);
    }
}
