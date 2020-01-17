namespace Core
{
    public class BaseModel
    {
        public float Health
        {
            get { return _health; }
        }
    
        private readonly float _health;

        public BaseModel(float health)
        {
            _health = health;
        }
    }
}