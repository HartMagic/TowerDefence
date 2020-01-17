using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class TowersController 
{
    public static TowersController Instance
    {
        get
        {
            if (_instance == null)
                _instance = new TowersController();

            return _instance;
        }
    }
    
    public IList<TowerBase> ActiveTowers
    {
        get { return _towersOnScene.Where(x => x.IsEnabled).ToList(); }
    }
    
    private static TowersController _instance;
    
    private readonly IList<TowerBase> _towersOnScene = new List<TowerBase>();
    private readonly IList<TowerBase> _towersForUnregistering = new List<TowerBase>();
    
    private Coroutine _updatingTowersCoroutine;

    public void StartUpdateTowers()
    {
        StopUpdateTowers();

        _updatingTowersCoroutine = SceneController.Instance.StartCoroutine(UpdateTowers());
    }

    public void StopUpdateTowers()
    {
        if (_updatingTowersCoroutine != null)
        {
            SceneController.Instance.StopCoroutine(_updatingTowersCoroutine);
            _updatingTowersCoroutine = null;
        }
    }
    
    public void RegisterTower(TowerBase unit)
    {
        if(!_towersOnScene.Contains(unit))
            _towersOnScene.Add(unit);
    }

    public void UnregisterTower(TowerBase unit)
    {
        if (_towersOnScene.Contains(unit))
        {
            _towersForUnregistering.Add(unit);
        }
    }

    private IEnumerator UpdateTowers()
    {
        while (true)
        {
            foreach (var activeTower in ActiveTowers)
            {
                activeTower.Update();
            }
            
            yield return new WaitForEndOfFrame();
            ClearUnitsForUnregistering();
        }
    }

    private void ClearUnitsForUnregistering()
    {
        foreach (var towerBase in _towersForUnregistering)
        {
            if (_towersOnScene.Contains(towerBase))
                _towersOnScene.Remove(towerBase);
        }
        
        _towersForUnregistering.Clear();
    }
}
