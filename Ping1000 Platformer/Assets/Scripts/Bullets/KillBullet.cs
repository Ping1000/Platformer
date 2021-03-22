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
        CharacterController2D player = collision.gameObject.
            GetComponent<CharacterController2D>();
        if (player != null) {
            player.HitPlayer(1);
            DespawnBullet();
        }
        // add damage for other things like enemies?
    }
}
