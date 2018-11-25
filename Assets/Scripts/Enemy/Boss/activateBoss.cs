using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class activateBoss : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        BossGolem.bossActive = true;
        BossGolem2.bossActive = true;
    }
}
