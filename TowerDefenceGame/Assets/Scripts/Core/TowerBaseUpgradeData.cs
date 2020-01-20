namespace Core
{
    public class TowerBaseUpgradeData : IUpgradeData
    {
        public float DetectingDistance
        {
            get { return _detectingDistance; }
        }

        public float Damage
        {
            get { return _damage; }
        }
        
        public int Cost
        {
            get { return _cost; }
        }
        
        private readonly float _damage;
        private readonly float _detectingDistance;
        private readonly int _cost;

        public TowerBaseUpgradeData(float damage, float detectingDistance, int cost)
        {
            _damage = damage;
            _detectingDistance = detectingDistance;
            _cost = cost;
        }
    }
}