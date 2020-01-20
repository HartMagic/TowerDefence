namespace Core
{
    public class DefaultTowerUpgradeData : TowerBaseUpgradeData
    {
        public float FiringRate
        {
            get { return _firingRate; }
        }
        
        private readonly float _firingRate;
        
        public DefaultTowerUpgradeData(float damage, float detectingDistance, float firingRate, int cost) : base(damage,
            detectingDistance, cost)
        {
            _firingRate = firingRate;
        }
    }
}