using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillVolume : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Fall Off");
        if (col.gameObject.CompareTag("Player")) 
        {
            LevelProgressTracker.PlayerDeath();
        }
        else
        {
            Destroy(col.gameObject);
        }
        Debug.Log("Fell Off");
    }
}
