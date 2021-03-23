using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NOTE: The collider should have isTrigger enabled.
// NOTE: The position of the checkpoint should be the position of the player
//       when they respawn at checkpoint. Can shift collider with offsets.

[RequireComponent(typeof(Collider2D))]
public class Checkpoint : MonoBehaviour
{
    private Collider2D _col;
    public Transform cameraPos;
    public float cameraSize;
    [Tooltip("List of objects to destroy after reaching a checkpoint." +
        "Useful for clearing out enemies etc.")]
    public List<GameObject> objectsToDestroy;
    // [Tooltip("Set this bool to what you want the game time state to be on restart.")]
    // public bool isPast;

    void Awake()
    {
        _col = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadFromCheckpoint(Transform player) {
        Vector3 playerPos = transform.position;
        playerPos.x += 0.5f;
        player.position = playerPos;
        _col.isTrigger = false;
        Camera.main.transform.position = cameraPos.position;
        Camera.main.orthographicSize = cameraSize;
        foreach (GameObject obj in objectsToDestroy) {
            Destroy(obj);
        }
        // TimeManager.SetTime(isPast);
    }

    public void MoveCamera() {
        Camera cam = Camera.main;

        LeanTween.value(cam.orthographicSize, cameraSize, 3.0f).setEaseInOutCubic().setOnUpdate((float f) => cam.orthographicSize = f);
        LeanTween.move(cam.gameObject, cameraPos.position, 3.0f).setEaseInOutCubic();
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            _col.isTrigger = false;
            LevelProgressTracker.UpdateLevelProgress();
            MoveCamera();
            foreach (GameObject obj in objectsToDestroy) {
                Destroy(obj);
            }
        }
    }
}
