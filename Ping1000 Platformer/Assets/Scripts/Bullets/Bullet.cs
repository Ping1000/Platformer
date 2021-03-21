using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public abstract class Bullet : MonoBehaviour
{
    public float moveSpeed;
    public float lifetime;

    protected Collider2D _col;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        _col = GetComponent<Collider2D>();
        _col.isTrigger = true; // maybe change? idk
    }

    private void OnEnable() {
        GetComponent<Rigidbody2D>().velocity = moveSpeed * transform.right;
    }

    // Update is called once per frame
    protected virtual void FixedUpdate()
    {
        if (lifetime <= 0) {
            DespawnBullet();
        }
        lifetime -= Time.fixedDeltaTime;
    }

    public virtual void DespawnBullet() {
        Destroy(gameObject); // do anything else? sound/visual effect, etc
    }

    protected abstract void OnTriggerEnter2D(Collider2D collision);
}
