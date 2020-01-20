namespace Core
{
    public abstract class UnitModel
    {
        public float Health
        {
            get { return _health; }
        }

        public float Damage
        {
            get { return _damage; }
        }

        public int Gold
        {
            get { return _gold; }
        }

        public float Speed
        {
            get { return _speed; }
        }
    
        private readonly float _health;
        private readonly float _damage;
        private readonly int _gold;
        private readonly float _speed;

        protected UnitModel(float health, float damage, int gold, float speed)
        {
            _health = health;
            _damage = damage;
            _gold = gold;
            _speed = speed;
        }
    }
}
