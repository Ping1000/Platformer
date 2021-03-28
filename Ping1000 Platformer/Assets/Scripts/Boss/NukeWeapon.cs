using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class NukeWeapon : MonoBehaviour
{
    private Collider2D _col;
    public float expandSpeed;

    // Start is called before the first frame update
    void Start()
    {
        _col = GetComponent<Collider2D>();
        _col.isTrigger = true;
        gameObject.transform.localScale = new Vector3(0, 0, 1);
    }

    // Update is called once per frame
    void Update()
    {
        float currentScale = gameObject.transform.localScale.x;
        currentScale += expandSpeed * Time.deltaTime;
        gameObject.transform.localScale = new Vector3(currentScale, currentScale, 1);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        EricCharacterMovement player;
        if (player = collision.gameObject.GetComponent<EricCharacterMovement>()) {
            player.HitPlayer(1000); // instakill
        }
    }

    private void OnDisable() {
        Destroy(gameObject);
    }
}
