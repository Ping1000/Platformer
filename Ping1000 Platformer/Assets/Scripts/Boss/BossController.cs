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
    private GameObject enragedWall;

    [SerializeField]
    private bool facingRight;
    [SerializeField]
    public Transform leapPosition;

    [HideInInspector]
    public bool isInvincible;
    [SerializeField] 
    private int invincibleTime = 3;

    [SerializeField]
    private Transform missileLaunchPoint;

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
        enragedWall = GetComponentInChildren<EnragedWall>().gameObject;
        enragedWall.SetActive(false);
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
        SFXManager.PlayNewSound("Audio/SFX/Boss_Loud_Landing", volumeType.half);
        MusicManager.instance.EnragedMusicBegin();

        // do something to level here? like in a coroutine?
        DoorLocks.LockDoors(false);
        _rb.velocity = Vector2.zero;
        Camera.main.GetComponent<FollowCam>().enabled = true;

        foreach (MissileController missile in FindObjectsOfType<MissileController>()) {
            Destroy(missile.gameObject);
        }
        SFXManager.instance.StopMissileTravel();

        // add the invisible wall that pushes the player forward
        enragedWall.SetActive(true);

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
        LeanTween.move(gameObject, info.bossPosition, 2f).setEaseOutSine();

        DoorLocks.LockDoors(true);
    }

    public void StopChasing() {
        isChasing = false;
        _rb.velocity = Vector2.zero;
        enragedWall.SetActive(false);
        MusicManager.instance.FinalMusicBegin();
        StartNewPhase();
    }

    public void HitBoss(int damage = 1) {
        //if (isInvincible)
        //    return;

        Health -= damage;
        _anim.SetTrigger("damaged");
        //if (Health > 0) {
        //    isInvincible = true;
        //    float halfFlashTime = 0.2f;
        //    LeanTween.alpha(gameObject, 0f, halfFlashTime).
        //        setLoopPingPong((int)(invincibleTime / (2 * halfFlashTime))).
        //        setOnComplete(() => isInvincible = false);
        //}
    }

    public void Die() {
        // something cool
        LevelProgressTracker.PlayerVictory();
        Destroy(gameObject);
    }

    public void LaunchMissile() {
        GameObject missile = Instantiate(Resources.Load("Prefabs/Missile") as GameObject,
            TimeManager.activeTimeParent);

        missile.transform.position = missileLaunchPoint.position;

        // point missile at player
        Vector3 newRot = missile.transform.rotation.eulerAngles;
        newRot.z = Vector3.SignedAngle(missile.transform.right, 
            player.transform.position - missile.transform.position, 
            Vector3.forward) + 90;
        missile.transform.rotation = Quaternion.Euler(newRot);

        SFXManager.instance.StartMissileSound();
    }

    public void LaunchNuke() {
        GameObject nuke = Instantiate(Resources.Load("Prefabs/Nuke") as GameObject,
            TimeManager.activeTimeParent);
        nuke.transform.position = transform.position;
    }

    public void FireGun() {
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
