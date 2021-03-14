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
        DespawnBullet();
    }
}
