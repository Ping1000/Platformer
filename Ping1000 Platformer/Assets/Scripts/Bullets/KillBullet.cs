using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBullet : Bullet
{

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void OnTriggerEnter2D(Collider2D collision) {
        Debug.Log(collision.gameObject.name);
        EricCharacterMovement player = collision.gameObject.
            GetComponent<EricCharacterMovement>();
        if (player != null) {
            player.HitPlayer();
            DespawnBullet();
        }
        // add damage for other things like enemies?
    }
}
