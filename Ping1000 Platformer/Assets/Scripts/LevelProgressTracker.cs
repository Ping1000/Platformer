﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelProgressTracker : MonoBehaviour 
{
    public Transform player;

    public static LevelProgressTracker instance;

    public List<Checkpoint> checkpoints;

    public static Dictionary<string, int> progressDict;

    private string sceneString;

    private void Awake() {
        instance = this;
        sceneString = SceneManager.GetActiveScene().name;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (progressDict == null)
            progressDict = new Dictionary<string, int>();
        if (!progressDict.ContainsKey(sceneString)) {
            progressDict[sceneString] = 0;
        } else {
            SetupCheckpoint();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) {
            PlayerDeath();
        }
    }

    public static void ResetLevelProgress(string levelName) {
        progressDict[levelName] = 0;
    }

    public static void UpdateLevelProgress(string levelName = "", int amount = 1) {
        if (levelName == "")
            levelName = instance.sceneString;

        progressDict[levelName] += amount;
    }

    public static void PlayerDeath() {
        // fade to black/death animation first
        // might have to move this to a coroutine then

        SceneManager.LoadScene(instance.sceneString);
    }

    private void SetupCheckpoint() {
        if (progressDict[sceneString] > 0) {
            // load to checkpoint
            Checkpoint checkpoint = checkpoints[progressDict[sceneString] - 1];
            checkpoint.LoadFromCheckpoint(player);
        } else {
            // load from start
        }
    }
}
