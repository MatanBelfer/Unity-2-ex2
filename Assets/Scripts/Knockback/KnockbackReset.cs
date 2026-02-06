using UnityEngine;

public class KnockbackReset : StateMachineBehaviour
{
    private static readonly int KB_Trigger = Animator.StringToHash("KB_Trigger");
    private static readonly int KB_Slow   = Animator.StringToHash("KB_Slow");
    private static readonly int KB_Speed  = Animator.StringToHash("KB_Speed");

    public override void OnStateExit(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex)
    {
        animator.ResetTrigger(KB_Trigger);
        animator.SetBool(KB_Slow, false);
        animator.SetFloat(KB_Speed, 1f);
    }
}
