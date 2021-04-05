using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wm_idle_behavior : StateMachineBehaviour
{
    [Tooltip("The minimum amount of time we can spend in idle.")]
    public float minIdleTime = 3f;
    [Tooltip("The maximum amount of time we can spend in idle.")]
    public float maxIdleTime = 7.5f;

    private float randIdleTime;
    private string[] options;
    private int optionChosen;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        randIdleTime = Random.Range(minIdleTime, maxIdleTime);
        options = new string[] {"move", "missile", "nuke", "gun"}; // jump excluded for now
        optionChosen = Random.Range(0, options.Length);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (randIdleTime <= 0) {
            animator.SetTrigger(options[optionChosen]);
        } else {
            randIdleTime -= Time.deltaTime;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

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
