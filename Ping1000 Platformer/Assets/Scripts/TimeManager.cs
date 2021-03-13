using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public KeyCode timeSwitch;
    public GameObject pastParent;
    public GameObject pastTilemap;
    public GameObject presentParent;
    public GameObject presentTilemap;

    public bool isPast;

    // Start is called before the first frame update
    void Start()
    {
        isPast = false;
        SetTime(isPast);
    }

    private void Update() {
        if (Input.GetKeyDown(timeSwitch)) {
            SwapTime();
        }
    }

    public void SwapTime() {
        isPast = !isPast;
        SetTime(isPast);
    }

    public void SetTime(bool isPast) {
        pastParent.SetActive(isPast);
        pastTilemap.SetActive(isPast);
        presentParent.SetActive(!isPast);
        presentTilemap.SetActive(!isPast);
    }
}
