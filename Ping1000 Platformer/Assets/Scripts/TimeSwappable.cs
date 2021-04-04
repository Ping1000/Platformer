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
    [Tooltip("The object to swap. Usually this GameObject or its parent.")]
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
        TimeManager.SwapTime(objectToSwap);
    }
}
