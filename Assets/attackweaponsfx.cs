using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackweaponsfx : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        SoundManager.instance.Play("SwordAttack");
    }
}
