using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public abstract class Bullet : MonoBehaviour
{
    public float moveSpeed;
    public float lifetime;
    [HideInInspector]
    public GameObject firedBy;

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

    /// <summary>
    /// Checks if a bullet collided with the object that fired it
    /// </summary>
    /// <param name="collision"></param>
    /// <returns></returns>
    protected bool CollidedWithSelf(Collider2D collision) {
        if (collision.gameObject == firedBy)
            return true;
        foreach (Collider2D child_col in collision.gameObject.
            GetComponentsInChildren<Collider2D>()) {
            if (child_col.gameObject == firedBy)
                return true;
        }
        return false;
    }

    protected abstract void OnTriggerEnter2D(Collider2D collision);
}
