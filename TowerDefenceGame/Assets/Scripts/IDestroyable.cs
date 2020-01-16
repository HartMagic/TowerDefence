using System;

public interface IDestroyable
{
    float Health { get; }
    bool IsDestroyed { get; }
    
    event Action<IDestroyable, float> Damaged;
    event Action<IDestroyable> Destroyed;
    
    void ApplyDamage(float damage);
}