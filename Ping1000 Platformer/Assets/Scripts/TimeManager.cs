using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [Header("Past")]
    [SerializeField] public bool isPast; // what should it set to on restart?
    [SerializeField] public GameObject pastParent;
    [SerializeField] public List<GameObject> pastTilemaps;
    
    [Header("Future")]
    [SerializeField] public GameObject futureParent;
    [SerializeField] public List<GameObject> futureTilemaps;

    [Header("Misc")]
    [Tooltip("When swapping time, how long the boss takes to come back.")]
    [SerializeField] public float bossHideTime;

    private BossController _bc;

    public static TimeManager instance;
    /// <summary>
    /// The parent Transform for the active time period. Useful
    /// when instantiating
    /// </summary>
    public static Transform activeTimeParent { get {
            if (instance.isPast)
                return instance.pastParent.transform;
            return instance.futureParent.transform;
        } }


    private void Awake() {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        SetTime(isPast);
        _bc = FindObjectOfType<BossController>();
    }

    /// <summary>
    /// Swaps the global time by enabling/disabling the parent
    /// past/future GameObjects and Tilemaps. Mostly used when
    /// the player's time needs to be changed.
    /// </summary>
    public static void SwapTime() {
        instance.isPast = !instance.isPast;
        SetTime(instance.isPast);
        instance.HideBoss();
        SFXManager.PlayNewSound("Audio/SFX/Time_Leap", volumeType.half);
    }

    /// <summary>
    /// Sends a GameObject into the other time.
    /// </summary>
    /// <param name="swappedObj">The object to send</param>
    public static void SwapTime(GameObject swappedObj) {
        if (instance.isPast) {
            swappedObj.transform.parent = instance.futureParent.transform;
        } else {
            swappedObj.transform.parent = instance.pastParent.transform;
        }
        SFXManager.PlayNewSound("Audio/SFX/Time_gun_impact", volumeType.half);
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
        foreach (GameObject tilemap in instance.pastTilemaps) {
            tilemap.SetActive(isPast);
        }
        instance.futureParent.SetActive(!isPast);
        foreach (GameObject tilemap in instance.futureTilemaps) {
            tilemap.SetActive(!isPast);
        }
    }

    public void HideBoss() {
        if (_bc && _bc.gameObject.activeInHierarchy) {
            _bc.gameObject.SetActive(false);
            Invoke("RevealBoss", bossHideTime);
        }
    }

    void RevealBoss() {
        if (_bc) {
            // play sound
            SFXManager.PlayNewSound("Audio/SFX/Time_gun_impact", volumeType.half,
                Random.Range(0.5f, 1.5f));
            _bc.gameObject.SetActive(true);
        }
    }
}
