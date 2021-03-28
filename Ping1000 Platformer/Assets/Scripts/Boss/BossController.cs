using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Rigidbody2D))]
public class BossController : MonoBehaviour {
    public GameObject missilePrefab;
    [HideInInspector]
    public int currentPhase = 0;
    public List<PhaseInfo> phaseInfo;
    public int TotalPhases { get {
            return phaseInfo.Count;
        } }

    private Animator _anim;
    private GunController _gun;
    private Rigidbody2D _rb;
    private bool isEnraged;

    [HideInInspector]
    public bool isChasing;
    public float chasingMoveSpeed;

    [SerializeField]
    private bool facingRight;
    [SerializeField]
    public Transform leapPosition;

    public int baseHealth { get; private set; }
    public int Health { get { return _anim.GetInteger("health"); } 
        set { _anim.SetInteger("health", value); }}

    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _gun = GetComponentInChildren<GunController>();
        baseHealth = Health;
        isChasing = false;
        StartNewPhase();
    }

    // Update is called once per frame
    void Update() {
        // maybe do some casing on info.isEnragedPhase
        if (isChasing) {
            Vector2 vel = _rb.velocity;
            vel.x = chasingMoveSpeed;
            _rb.velocity = vel;
        }
        // movement stuff here or in startnewPhase
    }

    private void Flip() {
        facingRight = !facingRight;

        transform.Rotate(new Vector3(0, 180, 0));
    }

    public void SetupChasing() {
        // setup (move to left/out of way, sfx, etc)
        LeapToPosition();
    }

    private void LeapToPosition() {
        float leapTime = 2f; // can make this an actual class var eventually

        LeanTween.moveX(gameObject, leapPosition.position.x, leapTime).setOnComplete(() =>
            BeginChasing());
        LeanTween.moveY(gameObject, transform.position.y + 10, leapTime / 2).
            setEaseOutExpo().setOnComplete(() =>
            LeanTween.moveY(gameObject, leapPosition.position.y, leapTime / 2).
            setEaseInExpo());
        // start moving right
    }

    private void BeginChasing() {
        if (!facingRight)
            Flip();

        // do something to level here? like in a coroutine?
        DoorLocks.LockDoors(false);
        Camera.main.GetComponent<FollowCam>().enabled = true;

        // start moving to the right
        isChasing = true;
    }

    private void StartNewPhase() {
        currentPhase++;

        PhaseInfo info = phaseInfo[currentPhase - 1];
        GameObject cam = Camera.main.gameObject;
        cam.GetComponent<FollowCam>().enabled = false;
        LeanTween.move(cam, info.cameraPos.position, 2f).setEaseOutSine();

        isEnraged = info.isEnragedPhase;
        if (currentPhase > 1)
            _anim.SetTrigger("idle");
        // do some setup stuff with the movement (set bounds, waypoints, or whatever)
        DoorLocks.LockDoors(true);
    }

    public void StopChasing() {
        isChasing = false;
        StartNewPhase();
    }

    public void HitBoss(int damage = 1) {
        Health -= damage;
        _anim.SetTrigger("damaged");
    }

    public void Die() {
        // something cool
        Destroy(gameObject);
    }

    public void LaunchMissile() {
        GameObject missile = Instantiate(missilePrefab, TimeManager.activeTimeParent);
        // maybe add a "launch position" like with the gun barrels
        missile.transform.position = gameObject.transform.position;
    }

    public void FireGun() {
        _gun.Shoot();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        EricCharacterMovement player;
        if (player = collision.gameObject.GetComponent<EricCharacterMovement>()) {
            player.HitPlayer();
            Vector2 pushDir = collision.contacts[0].point - (Vector2)transform.position;
            pushDir = pushDir.normalized;
            player.GetComponent<Rigidbody2D>().AddForce(pushDir * 1500);
        }
    }
}
