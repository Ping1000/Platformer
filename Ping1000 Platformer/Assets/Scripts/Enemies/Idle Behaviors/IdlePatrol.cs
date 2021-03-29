using System;
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
    [Header("Waypoint System (old)")]
    [SerializeField] bool useWaypoints = false;
    [SerializeField] List<Transform> movePositions;
    private int nextMovePos = 0;

    [Header("Detection System (new)")]
    [SerializeField] string floorTag = "Floor"; // NOT CURRENTLY USING TAGS, DON'T RELY ON THIS
    [SerializeField] LayerMask floorLayer;
    [SerializeField] float edgeDetectionDistance = .1f;
    [SerializeField] float wallDetectionDistance = .1f;
    [SerializeField] float wallDetectionMargins = .1f;
    [SerializeField] float objectDetectionDistance = .1f;
    [Space]
    [SerializeField] float haltTime = .5f;
    [SerializeField] float restartTime = .5f;


    Rigidbody2D _rb;
    Collider2D _collider;
    bool activePatrol;
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
    }

    void OnEnable()
    {
        StartCoroutine(Startup());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isIdle) {
            if (useWaypoints == false)
            {
                Patrol();
            }
            else
            {   // Old patrol code
                if (movePositions.Count == 0) { throw new InvalidOperationException("No movePositions given"); }
                if (Vector3.Distance(transform.position,
                movePositions[nextMovePos].position) > Mathf.Epsilon)
                {
                    transform.position = Vector3.MoveTowards(transform.position,
                        movePositions[nextMovePos].position, moveSpeed * Time.deltaTime);
                }
                else
                {
                    nextMovePos = (nextMovePos + 1) % movePositions.Count;
                    TryFlip();
                }
            }
            /**/
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

    /// <summary>
    /// Patrol. Move in a direction until an edge or wall is reached
    /// </summary>
    private void Patrol()
    {
        _rb.velocity = Vector2.zero;
        if (!activePatrol) { return; }

        // Check for edge or wall
        if (ObstructionFound())
        {
            // If edge or wall, halt patrol and turn around
            StartCoroutine(HaltPatrol());
        }
        else
        {
            // If not, move in direction
            PatrolMove();
        }




    }

    IEnumerator HaltPatrol()
    {
        //Debug.Log("Halting");
        activePatrol = false;
        yield return new WaitForSeconds(haltTime);
        TryFlip();
        yield return new WaitForSeconds(restartTime);
        activePatrol = true;
    }

    private void PatrolMove()
    {
        //Debug.Log("Moving");
        float dir = facingRight ? 1f : -1f;
        _rb.velocity = new Vector2(moveSpeed * dir, 0f);
    }

    private bool ObstructionFound()
    {
        return WallDetected() || EdgeDetected() || ObjectDetected();
        throw new NotImplementedException();
    }

    /// <summary>
    /// Object Found. 
    /// </summary>
    /// <returns> Returns true if an object is found </returns>
    private bool ObjectDetected()
    {
        return false;
        throw new NotImplementedException();
    }

    /// <summary>
    /// Edge Found.
    /// </summary>
    /// <returns> Returns true if an edge is found </returns>
    private bool EdgeDetected()
    {
       
        float dir = facingRight ? _collider.bounds.max.x : _collider.bounds.min.x;
        Vector2 edgeCheck = new Vector2(dir, _collider.bounds.min.y + edgeDetectionDistance);
        RaycastHit2D hit = Physics2D.Raycast(edgeCheck, Vector2.down, edgeDetectionDistance * 2, floorLayer);
        if (hit.collider == null)
        { // Not hitting anything, so assume over edge
            // Should I do anything else here?
            Debug.Log(name + " Hit Edge");
            return true;
        }
        else if (hit.collider != null && hit.collider.CompareTag(floorTag))
        { // Hit the floor, business as usual
            // Not currently working as platforms are not tagged
        }
        else
        { // Collider hit something, but not the floor
            //Debug.Log(name + " is edge detecting L: " + leftHit.collider.name + " R: " + rightHit.collider.name);
        }

        return false;
        
    }

    /// <summary>
    /// Wall Found.
    /// </summary>
    /// <returns> Returns true if a wall is encountered. </returns>
    private bool WallDetected()
    {
        // Boxcast extends wallDetectionDistance in direction of movement, is level with top bounds,
        // and is wallDetectionMargins above lower bounds
        float dir = facingRight ? 1f : -1f;
        Vector2 boxCenter = new Vector2(_collider.bounds.center.x + wallDetectionDistance * dir, 
                                        _collider.bounds.center.y + wallDetectionMargins);
        Vector2 boxSize = new Vector2(_collider.bounds.size.x + wallDetectionDistance, 
                                      _collider.bounds.size.y - wallDetectionMargins * 2);

        // Probably should use OverlapBox as that doesn't alloc, but this is easier
        Collider2D[] colliders = Physics2D.OverlapBoxAll(boxCenter, boxSize, 0f, floorLayer);
        
        if (colliders.Length > 0)
        { // Hit something
            Debug.Log(name + " Hit Wall, colliders hit: " + colliders.Length);
            return true;
        }

        return false;
    }

    protected override void TryFlip() {

        if (useWaypoints)
        {
            if (facingRight && transform.position.x > movePositions[nextMovePos].position.x)
                Flip();
            else if (!facingRight && transform.position.x < movePositions[nextMovePos].position.x)
                Flip();
        }
        else
        {
            Flip();
        }
    }

    IEnumerator Startup() {
        yield return new WaitForSeconds(restartTime);
        if (!isAggro)
        {
            isIdle = true;
            activePatrol = true;
        }
            
    }
}
