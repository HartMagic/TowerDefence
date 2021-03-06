﻿using System.Collections;

namespace Core
{
    public abstract class TowerModel
    {
        public float Damage
        {
            get { return _damage; }
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
        private readonly float _detectingDistance;
        private readonly int _cost;

        public TowerModel(float damage, float detectingDistance,int cost)
        {
            _damage = damage;
            _detectingDistance = detectingDistance;
            _cost = cost;
        }
    }
}
