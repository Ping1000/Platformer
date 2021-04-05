using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFightStart : MonoBehaviour
{
    public List<GameObject> objectsToActivate;
    public List<MonoBehaviour> componentsToEnable;

    private void Start() {
        foreach (GameObject go in objectsToActivate) {
            go.SetActive(false);
        }
        foreach (MonoBehaviour mb in componentsToEnable) {
            mb.enabled = false;
        }
    }

    public void StartFight() {
        foreach (GameObject go in objectsToActivate) {
            go.SetActive(true);
        }
        foreach (MonoBehaviour mb in componentsToEnable) {
            mb.enabled = true;
        }
    }
}
