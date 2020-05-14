using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackSFX : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        SoundManager.instance.Play("PushSFX");
    }
}
