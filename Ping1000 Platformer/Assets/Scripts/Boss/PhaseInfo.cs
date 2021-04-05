using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores information about a given boss phase.
/// </summary>
[System.Serializable]
public class PhaseInfo {
    public Transform cameraPos;
    public bool isEnragedPhase;
    public float bossMoveSpeed;
    // add boss movement/position info?
    // either a list of points or scripts or whatever
    public PhaseInfo() {
        cameraPos = null;
        isEnragedPhase = false;
        bossMoveSpeed = 3f;
    }
}
