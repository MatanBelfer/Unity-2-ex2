using UnityEngine;

public class KnockbackAnimatorDriver : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [Header("Slow")]
    [SerializeField] private float slowThreshold = 1.65f;

    [Header("Speed Settings")]
    [SerializeField] private float fastSpeed = 1.2f;
    [SerializeField] private float slowSpeed = 0.85f;

    private static readonly int KB_Trigger = Animator.StringToHash("KB_Trigger");
    private static readonly int KB_Slow  = Animator.StringToHash("KB_Slow");
    private static readonly int KB_Speed  = Animator.StringToHash("KB_Speed");

    public void Reset()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void PlayKnockback(float impactSpeed)
    {
        Debug.Log("PlayKnockback CALLED", this);
        bool slow = impactSpeed <= slowThreshold;

        animator.SetBool(KB_Slow, slow);
        animator.SetFloat(KB_Speed, slow ? fastSpeed : slowSpeed);
        
        animator.ResetTrigger(KB_Trigger);
        animator.SetTrigger(KB_Trigger);
    }
}