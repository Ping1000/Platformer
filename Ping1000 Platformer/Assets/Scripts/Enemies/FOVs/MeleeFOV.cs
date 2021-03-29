using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class MeleeFOV : EnemyFOV 
{
    [SerializeField]
    protected EnemyIdle _enemy;
    private Collider2D _col;
    private bool isAttacking;

    // Start is called before the first frame update
    void Start()
    {
        _col = GetComponent<Collider2D>();
        _col.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.CompareTag("Player") && !isAttacking) {
            _enemy.isAggro = true;
            _enemy.isIdle = false;
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack() {
        isAttacking = true;
        _enemy.isIdle = false;
        // pew pew here
        while (_enemy._anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
            yield return null;

        isAttacking = false;
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (enabled)
            StartCoroutine(WaitForAttack());
    }

    IEnumerator WaitForAttack() {
        while (isAttacking)
            yield return new WaitForEndOfFrame();
        _enemy.isAggro = false;
        _enemy.isIdle = true;
    }

    private void OnDisable() {
        _enemy.isAggro = false;
        _enemy.isIdle = true;
    }
}
