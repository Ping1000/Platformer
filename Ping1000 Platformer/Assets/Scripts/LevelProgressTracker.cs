using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelProgressTracker : MonoBehaviour
{
    public static LevelProgressTracker instance;

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
        
    }

    public static void PlayerDeath() {
        // fade to black/death animation first
        // might have to move this to a coroutine then

        SceneManager.LoadScene(instance.sceneString);
    }

    private void SetupCheckpoint() {
        if (progressDict[sceneString] > 0) {
            // load to checkpoint
        } else {
            // load from start
        }
    }
}
