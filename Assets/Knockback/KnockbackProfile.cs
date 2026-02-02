using UnityEngine;

[CreateAssetMenu(
    fileName = "KnockbackProfile",
    menuName = "Combat/Knockback Profile"
)]
public class KnockbackProfile : ScriptableObject
{
    [Header("Impact Speed > Knockback Distance")]
    public AnimationCurve distanceByImpactSpeed =
        AnimationCurve.Linear(0, 0, 10, 2);

    [Header("Impact Speed > Knockback Duration")]
    public AnimationCurve durationByImpactSpeed =
        AnimationCurve.Linear(0, 0.05f, 10, 0.25f);

    [Header("Filters")]
    public float minImpactSpeed = 0.5f;
    public float maxDistance = 2f;
    public float maxDuration = 0.35f;

    [Header("Movement Ease")]
    public AnimationCurve displacementEase =
        AnimationCurve.EaseInOut(0, 0, 1, 1);
    
    [Header("Hit Stun")]
    public AnimationCurve stunByImpactSpeed =
        AnimationCurve.Linear(0, 0.1f, 6, 0.4f);

    public float maxStun = 0.5f;
}