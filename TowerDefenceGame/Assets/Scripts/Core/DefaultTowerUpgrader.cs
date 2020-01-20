using Settings;

namespace Core
{
    public class DefaultTowerUpgrader : IUpgrader
    {
        private readonly TowerSettings _upgradeSettings;
        
        public DefaultTowerUpgrader(TowerSettings upgradeSettings)
        {
            _upgradeSettings = upgradeSettings;
        }

        public void Upgrade(ICanUpgrade target)
        {
            var data = GetUpgradeData(target);
            target.Upgrade(data);
        }

        public IUpgradeData GetUpgradeData(ICanUpgrade target)
        {
            var tower = target as DefaultTower;
            if (tower != null)
            {
                var data = GetData(tower);

                return data;
            }

            return null;
        }

        private DefaultTowerUpgradeData GetData(DefaultTower tower)
        {
            return new DefaultTowerUpgradeData(tower.Damage + _upgradeSettings.Damage*tower.Level, tower.DetectingDistance + _upgradeSettings.DetectingDistance*tower.Level,
                tower.FiringRate + _upgradeSettings.FiringRate*tower.Level, tower.Cost + _upgradeSettings.Cost*tower.Level);
        }
    }
}

