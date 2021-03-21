using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for simple enemy back-and-forth patrolling
/// </summary>
public class IdlePatrol : EnemyIdle
{
    /// <summary>
    /// List of positions to cycle through when moving
    /// </summary>
    public List<Transform> movePositions;
    private int nextMovePos = 0;
    public AttackBehavior _atk;

    // Start is called before the first frame update
    void OnEnable()
    {
        StartCoroutine(Startup());
    }

    // Update is called once per frame
    void Update()
    {
        if (isIdle) {
            if (Vector3.Distance(transform.position, 
                movePositions[nextMovePos].position) > Mathf.Epsilon) {
                transform.position = Vector3.MoveTowards(transform.position, 
                    movePositions[nextMovePos].position, moveSpeed * Time.deltaTime);
            } else {
                nextMovePos = (nextMovePos + 1) % movePositions.Count;
                TryFlip();
            }
        }
    }

    /// <summary>
    /// Start attack. Needs to be on this script because the animator needs to see it
    /// </summary>
    private void BeginAttack() {
        _atk.BeginAttack();
    }

    /// <summary>
    /// End attack. Needs to be on this script because the animator needs to see it
    /// </summary>
    private void EndAttack() {
        _atk.EndAttack();
    }

    protected override void TryFlip() {
        if (facingRight && transform.position.x > movePositions[nextMovePos].position.x)
            Flip();
        else if (!facingRight && transform.position.x < movePositions[nextMovePos].position.x)
            Flip();
    }

    IEnumerator Startup() {
        yield return new WaitForSeconds(0.5f);
        if (!isAggro)
            isIdle = true;
    }
}
