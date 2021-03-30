using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackBehavior : MonoBehaviour
{
    [HideInInspector]
    public bool isAttacking;
    public abstract void BeginAttack(string soundPath = "", 
        volumeType volume = volumeType.half);
    public abstract void EndAttack();
}
