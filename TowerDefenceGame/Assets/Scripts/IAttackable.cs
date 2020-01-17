public interface IAttackable
{
    IAttackTarget AttackTarget { get; }
    
    void Attack();
}