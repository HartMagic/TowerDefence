using Core;

public class DefaultTowerModel : TowerModel
{
    public float FiringRate
    {
        get { return _firingRate; }
    }
    
    private readonly float _firingRate;
    
    public DefaultTowerModel(float damage, float firingRate, float detectingDistance, int cost) : base(damage, detectingDistance, cost)
    {
        _firingRate = firingRate;
    }
}