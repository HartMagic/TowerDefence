using System;
using UnityEngine;

// Default unit factory
public class DefaultUnitSpawner
{
    private readonly IFactory<UnitVisual, UnitVisual.UnitVisualFactoryData> _defaultUnitVisualFactory;
    private readonly IFactory<UnitBase, UnitVisual, UnitModel, UnitPath> _defaultUnitFactory;

    public DefaultUnitSpawner(IFactory<UnitVisual, UnitVisual.UnitVisualFactoryData> defaultUnitVisualFactory,
        IFactory<UnitBase, UnitVisual, UnitModel, UnitPath> defaultUnitFactory)
    {
        _defaultUnitVisualFactory = defaultUnitVisualFactory;
        _defaultUnitFactory = defaultUnitFactory;
    }

    public UnitBase Spawn(UnitModel model, UnitPath path, Transform parent)
    {
        var pathMapper = new UnitPath.UnitPathMapper(path);
        var data = new UnitVisual.UnitVisualFactoryData(pathMapper.MapPoint(0.0f), Quaternion.LookRotation(pathMapper.MapDirection(0.0f)), parent);
        
        var unitVisual = _defaultUnitVisualFactory.Create(data);
        var unit = _defaultUnitFactory.Create(unitVisual, model, path);

        return unit;
    }
}
