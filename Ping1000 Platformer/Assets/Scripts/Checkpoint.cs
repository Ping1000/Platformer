using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NOTE: The collider should have isTrigger enabled.
// NOTE: The position of the checkpoint should be the position of the player
//       when they respawn at checkpoint. Can shift collider with offsets.

[RequireComponent(typeof(BoxCollider2D))]
public class Checkpoint : MonoBehaviour
{
    private BoxCollider2D _col;
    public Transform cameraPos;
    public float cameraSize;
    [Tooltip("List of objects to destroy after reaching a checkpoint." +
        "Useful for clearing out enemies etc.")]
    public List<GameObject> objectsToDestroy;
    // [Tooltip("Set this bool to what you want the game time state to be on restart.")]
    // public bool isPast;

    private float xOffset = 1f;

    void Awake()
    {
        _col = GetComponent<BoxCollider2D>();
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

        Vector2 newOffset = _col.offset;
        newOffset.x -= xOffset;
        _col.offset = newOffset;

        Camera.main.transform.position = cameraPos.position;
        Camera.main.orthographicSize = cameraSize;
        foreach (GameObject obj in objectsToDestroy) {
            Destroy(obj);
        }
        // TimeManager.SetTime(isPast);
    }

    public void MoveCamera() {
        Camera cam = Camera.main;

        LeanTween.value(cam.orthographicSize, cameraSize, 1.5f).setEaseInOutCubic().setOnUpdate((float f) => { 
            if (cam != null)
                cam.orthographicSize = f; 
        });
        LeanTween.move(cam.gameObject, cameraPos.position, 1.5f).setEaseInOutCubic();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            LevelProgressTracker.UpdateLevelProgress();
            MoveCamera();
            foreach (GameObject obj in objectsToDestroy) {
                Destroy(obj);
            }
            _col.enabled = false;
            AddInvisibleWall();
        }
    }

    private void AddInvisibleWall() {
        GameObject wall = Instantiate(Resources.Load<GameObject>("Prefabs/Invisible Wall"));
        Vector3 wallPos = transform.position + (Vector3)_col.offset;
        wallPos.x -= xOffset;
        wall.transform.position = wallPos;
    }
}
