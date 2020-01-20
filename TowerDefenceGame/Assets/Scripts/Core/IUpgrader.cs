namespace Core
{
    public interface IUpgrader
    {
        void Upgrade(ICanUpgrade target);
        IUpgradeData GetUpgradeData(ICanUpgrade target);
    }
}