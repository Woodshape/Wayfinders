using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynchedAnimation : MonoBehaviour
{
    //The animator controller attached to this GameObject
    public Animator _myAnimator;

    //Records the animation state or animation that the Animator is currently in
    public AnimatorStateInfo animatorStateInfo;

    //Used to address the current state within the Animator using the Play() function
    public int currentState;

    void Start()
    {
        //Load the animator attached to this object
        _myAnimator = GetComponent<Animator>();

        //Get the info about the current animator state
        animatorStateInfo = _myAnimator.GetCurrentAnimatorStateInfo(0);

        //Convert the current state name to an integer hash for identification
        currentState = animatorStateInfo.fullPathHash;
    }

    void Update()
    {
        //Start playing the current animation from wherever the current conductor loop is
        _myAnimator.Play(currentState, -1, (Conductor.Instance.loopPositionInAnalog));

        //Set the speed to 0 so it will only change frames when you next update it
        _myAnimator.speed = 0;
    }
}
