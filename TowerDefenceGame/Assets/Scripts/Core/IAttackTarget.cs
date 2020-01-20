using System;
using UnityEngine;

namespace Core
{
    public interface IAttackTarget
    {
        float Health { get; }
        float MaxHealth { get; }
        
        bool IsDestroyed { get; }
    
        Vector3 WorldPosition { get; }
    
        event Action<IAttackTarget, float> Damaged;
        event Action<IAttackTarget> Destroyed;
    
        void ApplyDamage(float damage);
    }
}
