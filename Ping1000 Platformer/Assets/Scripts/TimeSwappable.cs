using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
/// <summary>
/// Acts as a psuedo-interface for objects that can be shot with time bullets.
/// Using this instead of tags because some objects might have a parent
/// that needs to be transported, and not the object itself.
/// </summary>
public class TimeSwappable : MonoBehaviour
{
    [SerializeField]
    private GameObject objectToSwap;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwapTime() {
        SFXManager.PlayNewSound("Audio/SFX/Time_gun_impact", volumeType.half);
        TimeManager.SwapTime(objectToSwap);
    }
}
