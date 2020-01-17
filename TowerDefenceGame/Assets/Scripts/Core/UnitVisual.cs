using UnityEngine;

namespace Core
{
    public abstract class UnitVisual : MonoBehaviour
    {
        public virtual void InitializeVisual()
        {
            gameObject.SetActive(true);
        }

        public virtual void ApplyDamageVisual()
        {
        }
    
        public virtual void ApplyDestroyVisual()
        {
        }
    
        public virtual void ResetVisual()
        {
        }
    }
}
