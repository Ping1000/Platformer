using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class KartTestFMOD : MonoBehaviour
{
    StudioEventEmitter emitter;
    Rigidbody2D rb;
    [Range(0,1)]
    public float interpSpeed = 1;

    // Start is called before the first frame update
    void Start()
    {
        emitter = GetComponent<StudioEventEmitter>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        emitter.SetParameter("RPM", rb.velocity.magnitude * 150);
    }
}
