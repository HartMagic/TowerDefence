using System;
using UnityEngine;

namespace Core
{
    public abstract class TowerVisual : MonoBehaviour
    {
        public Bounds VisualBounds
        {
            get { return GetVisualBounds(gameObject); }
        }
        
        public virtual void InitializeVisual()
        {
        }

        public virtual void UpdateVisualTarget(Vector3 position)
        {
        }

        public virtual void UpdateUpgradeVisual(int level)
        {
        }

        public virtual void UpdateSelectedVisual(bool value)
        {
        }

        public virtual void ApplyAttackVisual()
        {
        }

        public virtual void ResetVisual()
        {
        }

        private Bounds GetVisualBounds(GameObject target)
        {
            var bounds = GetRendererBounds(target);

            if (Math.Abs(bounds.extents.x) < float.Epsilon)
            {
                bounds = new Bounds(target.transform.position, Vector3.zero);
                foreach (Transform child in target.transform)
                {
                    var childRenderer = child.GetComponent<Renderer>();

                    bounds.Encapsulate(childRenderer != null ? childRenderer.bounds : GetVisualBounds(child.gameObject));
                }
            }

            return bounds;
        }

        private Bounds GetRendererBounds(GameObject target)
        {
            var bounds = new Bounds(Vector3.zero, Vector3.zero);
            var targetRenderer = target.GetComponent<Renderer>();
            
            if (targetRenderer != null)
                return targetRenderer.bounds;

            return bounds;
        }

        protected virtual void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(VisualBounds.center, VisualBounds.size);
        }
    }
}



