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
    
        private readonly float _damage;
        private readonly float _detectingDistance;

        protected TowerModel(float damage, float detectingDistance)
        {
            _damage = damage;
            _detectingDistance = detectingDistance;
        }
    }
}
