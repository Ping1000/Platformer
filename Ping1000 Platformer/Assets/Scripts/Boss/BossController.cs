using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Rigidbody2D))]
public class BossController : MonoBehaviour {
    [HideInInspector]
    public int currentPhase = 0;
    public List<PhaseInfo> phaseInfo;
    public int TotalPhases { get {
            return phaseInfo.Count;
        } }

    private Animator _anim;
    private GunController _gun;
    private Rigidbody2D _rb;
    private EricCharacterMovement player;
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
        player = FindObjectOfType<EricCharacterMovement>();
        baseHealth = Health;
        isChasing = false;
        StartNewPhase();
    }

    // Update is called once per frame
    void Update() {
        FlipToPlayer();
        // maybe do some casing on info.isEnragedPhase
        if (isChasing) {
            Vector2 vel = _rb.velocity;
            vel.x = chasingMoveSpeed;
            _rb.velocity = vel;
        }
        // movement stuff here or in startnewPhase
    }

    private void FlipToPlayer() {
        if (transform.position.x < player.transform.position.x && !facingRight)
            Flip();
        if (transform.position.x > player.transform.position.x && facingRight)
            Flip();
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

        SFXManager.PlayNewSound("Audio/SFX/Jump", volumeType.half);
        LeanTween.moveX(gameObject, leapPosition.position.x, leapTime).setOnComplete(() =>
            BeginChasing());
        LeanTween.moveY(gameObject, transform.position.y + 10, leapTime / 2).
            setEaseOutExpo().setOnComplete(() =>
            LeanTween.moveY(gameObject, leapPosition.position.y, leapTime / 2).
            setEaseInExpo());
        // start moving right
    }

    private void BeginChasing() {
        // TEMPORARYYYY
        SFXManager.PlayNewSound("Audio/SFX/Boss_Loud_Landing", volumeType.half);
        //if (!facingRight)
        //    Flip();

        // do something to level here? like in a coroutine?
        DoorLocks.LockDoors(false);
        _rb.velocity = Vector2.zero;
        GetComponent<BossFlyingMovement>().enabled = false;
        Camera.main.GetComponent<FollowCam>().enabled = true;

        // start moving to the right
        isChasing = true;
    }

    private void StartNewPhase() {
        currentPhase++;

        PhaseInfo info = phaseInfo[currentPhase - 1];
        GameObject cam = Camera.main.gameObject;
        cam.GetComponent<FollowCam>().enabled = false;
        GetComponent<BossFlyingMovement>().enabled = true;
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
        GameObject missile = Instantiate(Resources.Load("Prefabs/Missile") as GameObject,
            TimeManager.activeTimeParent);

        missile.transform.position = transform.Find("Missile Launch Point").position;

        SFXManager.instance.StartMissileSound();
    }

    public void LaunchNuke() {
        GameObject nuke = Instantiate(Resources.Load("Prefabs/Nuke") as GameObject,
            TimeManager.activeTimeParent);
        nuke.transform.position = transform.position;
    }

    public void FireGun() {
        // flip to be facing toward the player?
        _gun.Shoot("Audio/SFX/Boss_Gun", volumeType.quiet);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.GetComponent<EricCharacterMovement>() == player) {
            player.HitPlayer();
            Vector2 pushDir = collision.contacts[0].point - (Vector2)transform.position;
            pushDir = pushDir.normalized;
            player.GetComponent<Rigidbody2D>().AddForce(pushDir * 1500);
        }
    }
}
