using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wm_damaged_behavior : StateMachineBehaviour
{
    private BossController bc;
    private bool becameEnraged;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        bc = animator.GetComponent<BossController>();
        becameEnraged = false;
        if (bc != null) {
            if (bc.Health <= 0) {
                if (bc.currentPhase == bc.TotalPhases) {
                    bc.Die();
                } else {
                    becameEnraged = true;
                    animator.SetTrigger("enraged");
                }
            } else {
                // go back to idle
                // already handled in the animation transitions
            }
        }
        else
            Debug.LogError("No BossController found on animator. (damaged behavior)");
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (becameEnraged)
            bc.Health = bc.baseHealth;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
