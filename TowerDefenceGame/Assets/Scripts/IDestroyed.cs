using System;

public interface IDestroyed
{
    bool IsDestroyed { get; }
    
    event Action<IDestroyed, float> Damaged;
    event Action<IDestroyed> Destroyed;
    
    void ApplyDamage(float damage);
}