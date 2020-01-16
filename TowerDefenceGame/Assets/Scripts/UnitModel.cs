public class UnitModel
{
    public float Health
    {
        get { return _health; }
    }

    public float Damage
    {
        get { return _damage; }
    }

    public float Gold
    {
        get { return _gold; }
    }

    public float Speed
    {
        get { return _speed; }
    }
    
    private float _health;
    private float _damage;
    private float _gold;
    private float _speed;

    public UnitModel(float health, float damage, float gold, float speed)
    {
        _health = health;
        _damage = damage;
        _gold = gold;
        _speed = speed;
    }
}