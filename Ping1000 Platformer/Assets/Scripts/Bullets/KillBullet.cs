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
        EricCharacterMovement player = collision.gameObject.
            GetComponent<EricCharacterMovement>();
        if (player != null) {
            player.HitPlayer(1);
            DespawnBullet();
        }
        // add damage for other things like enemies?
    }
}
