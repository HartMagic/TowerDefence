using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Core
{
    public sealed class TowerSelector : ISelector
    {
        private readonly Camera _camera;
        private readonly GraphicRaycaster _graphicRaycaster;

        private ICanSelect _currentTower;
        
        public TowerSelector(Camera camera, GraphicRaycaster graphicRaycaster)
        {
            _camera = camera;
            _graphicRaycaster = graphicRaycaster;
        }
        
        public ICanSelect Select(IEnumerable<ICanSelect> selectableItems)
        {
            if (CheckUIRaycast(Input.mousePosition))
                return _currentTower;
            
            var towers = selectableItems.Cast<TowerBase>();
            
            var distance = float.MaxValue;
            var ray = _camera.ScreenPointToRay(Input.mousePosition);

            TowerBase closestTower = null;

            foreach (var tower in towers)
            {
                var bounds = tower.VisualBounds;
                float newDistance;
                
                if (bounds.IntersectRay(ray, out newDistance))
                {
                    if (newDistance < distance)
                    {
                        closestTower = tower;
                        distance = newDistance;
                    }
                }
            }
            
            if (_currentTower != null && _currentTower != closestTower)
            {
                _currentTower.Unselect();
            }
            
            _currentTower = closestTower;

            if (closestTower != null)
            {
                closestTower.Select();
            }

            return closestTower;
        }

        private bool CheckUIRaycast(Vector2 position)
        {
            var data = new PointerEventData(EventSystem.current);
            data.position = position;
            var result = new List<RaycastResult>();
            
            _graphicRaycaster.Raycast(data, result);

            return result.Count > 0;
        }
    }
}