using UnityEngine;

namespace Core
{
    public abstract class TowerVisual : MonoBehaviour
    {
        public virtual void InitializeVisual()
        {
        }

        public virtual void UpdateVisualTarget(Vector3 position)
        {
        }

        public virtual void ApplyAttackVisual()
        {
        
        }

        public virtual void ResetVisual()
        {
        }
    }
}



