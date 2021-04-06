using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSoundController : MonoBehaviour
{
    private AudioSource _src;
    private Rigidbody2D _rb;

    private void Start() {
        _src = GetComponent<AudioSource>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.relativeVelocity.y > 5f) {
            SFXManager.PlayNewSound("Audio/SFX/Misc/Box_Landing", volumeType.half);
        }
    }

    private void OnCollisionStay2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player") &&
            Mathf.Abs(_rb.velocity.x) > 0.1f) {
            _src.volume = Mathf.Clamp(Mathf.Abs(_rb.velocity.x) / 5f, 0, 1);
        } else {
            _src.volume = 0f;
        }
    }
}
