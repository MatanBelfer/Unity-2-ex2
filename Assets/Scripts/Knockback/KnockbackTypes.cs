using UnityEngine;

public interface Iknockbackable

{
    void ApplyKnockback(KnockbackRequest request);
    bool IsInKnockback { get; }
    
}

public readonly struct KnockbackRequest
{
    
    //variables of the knockback request
    public readonly Vector3 Direction;
    public readonly float Distance;
    public readonly float Duration;
    public readonly Vector3 SourcePoint;
    public readonly GameObject Source;
    
    
//constructor of struct
    public KnockbackRequest(Vector3 direction, float distance, float duration, Vector3 sourcePoint, GameObject source)
    {
        Direction = direction;
        Distance = distance;
        Duration = duration;
        SourcePoint = sourcePoint;
        Source = source;
    }
}
