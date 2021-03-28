using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EnragedEnd : MonoBehaviour
{
    private Collider2D _col;

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

    private void OnTriggerExit2D(Collider2D collision) {
        BossController bc;
        if (bc = collision.gameObject.GetComponent<BossController>()) {
            bc.StopChasing();
        }
        // do something to the environment (lock the doors etc)
    }
}
