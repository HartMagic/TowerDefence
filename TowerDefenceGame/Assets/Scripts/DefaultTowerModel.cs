using Core;

public class DefaultTowerModel : TowerModel
{
    public float FiringRate
    {
        get { return _firingRate; }
    }
    
    public int Cost
    {
        get { return _cost; }
    }
    
    private readonly float _firingRate;
    private readonly int _cost;
    
    public DefaultTowerModel(float damage, float firingRate, float detectingDistance, int cost) : base(damage, detectingDistance)
    {
        _firingRate = firingRate;
        _cost = cost;
    }
}