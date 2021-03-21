using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public abstract class EnemyIdle : MonoBehaviour
{
    [Tooltip("Trigger for the aggro field of view.")]
    public PolygonCollider2D fieldOfView;
    public float moveSpeed;

    public Animator _anim;
    [SerializeField] protected bool facingRight;

    public bool isIdle {
        get {
            return _anim.GetBool("isIdle");
        }
        set {
            _anim.SetBool("isIdle", value);
        }
    }
    public bool isAggro {
        get {
            return _anim.GetBool("isAggro");
        }
        set {
            _anim.SetBool("isAggro", value);
        }

    }

    /// <summary>
    /// Try to flip based on conditions set by the children.
    /// </summary>
    protected abstract void TryFlip();

    protected void Flip() {
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;

        // Rotate about the y-axis by 180 degrees.
        transform.Rotate(new Vector3(0, 180, 0));
    }
}
