using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerModel
{
    public float Damage
    {
        get { return _damage; }
    }

    public float FiringRate
    {
        get { return _firingRate; }
    }

    public float DetectingDistance
    {
        get { return _detectingDistance; }
    }

    public int Cost
    {
        get { return _cost; }
    }
    
    private readonly float _damage;
    private readonly float _firingRate;
    private readonly float _detectingDistance;
    private readonly int _cost;

    public TowerModel(float damage, float firingRate, float detectingDistance, int cost)
    {
        _damage = damage;
        _firingRate = firingRate;
        _detectingDistance = detectingDistance;
        _cost = cost;
    }
}
