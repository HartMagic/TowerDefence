using System;

namespace Core
{
    public interface ICanUpgrade
    {
        int Cost { get; }
        int Level { get; }

        event Action<int> Upgraded; 
        
        void Upgrade(IUpgradeData data);
    }
}