using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class UnitsController : MonoBehaviour
{
    public List<UnitBase> ActiveUnits
    {
        get { return _unitsOnScene.Where(x => !x.IsDestroyed).ToList(); }
    }
    
    private readonly List<UnitBase> _unitsOnScene = new List<UnitBase>();

    private readonly List<UnitBase> _unitsForUnregistering = new List<UnitBase>();
    
    public static UnitsController Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<UnitsController>();

            return _instance;
        }
    }
    
    private static UnitsController _instance;

    private Coroutine _updatingUnitsCoroutine;

    private void Awake()
    {
        StartUpdateUnits();
    }

    public void StartUpdateUnits()
    {
        StopUpdateUnits();

        _updatingUnitsCoroutine = StartCoroutine(UpdateUnits());
    }

    public void StopUpdateUnits()
    {
        if (_updatingUnitsCoroutine != null)
        {
            StopCoroutine(_updatingUnitsCoroutine);
            _updatingUnitsCoroutine = null;
        }
    }
    
    public void RegisterUnit(UnitBase unit)
    {
        if(!_unitsOnScene.Contains(unit))
            _unitsOnScene.Add(unit);
    }

    public void UnregisterUnit(UnitBase unit)
    {
        if (_unitsOnScene.Contains(unit))
        {
            _unitsForUnregistering.Add(unit);
        }
    }

    private IEnumerator UpdateUnits()
    {
        while (true)
        {
            foreach (var activeUnit in ActiveUnits)
            {
                activeUnit.Update();
            }
            
            yield return new WaitForEndOfFrame();
        }
    }

    private void LateUpdate()
    {
        foreach (var baseUnit in _unitsForUnregistering)
        {
            if (_unitsOnScene.Contains(baseUnit))
                _unitsOnScene.Remove(baseUnit);
        }
        
        _unitsForUnregistering.Clear();
    }
}
