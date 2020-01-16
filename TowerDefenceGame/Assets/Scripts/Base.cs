using System;
using UnityEngine;

public class Base : MonoBehaviour, IDestroyed, IDamageTarget
{
    public float Health { get; }
    
    public bool IsDestroyed { get; }
    
    public event Action<IDestroyed, float> Damaged;
    public event Action<IDestroyed> Destroyed;

    public void ApplyDamage(float damage)
    {
        throw new System.NotImplementedException();
    }
}

public interface IDamageTarget
{
    float Health { get; }
}