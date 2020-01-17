using Core;
using UnityEngine;

// Default unit factory
public class DefaultUnitSpawner
{
    private readonly IFactory<DefaultUnitVisual, DefaultUnitVisual.DefaultUnitVisualFactoryData> _defaultUnitVisualFactory;
    private readonly IFactory<DefaultUnit, DefaultUnitVisual, DefaultUnitModel, UnitPath> _defaultUnitFactory;

    public DefaultUnitSpawner(IFactory<DefaultUnitVisual, DefaultUnitVisual.DefaultUnitVisualFactoryData> defaultUnitVisualFactory,
        IFactory<DefaultUnit, DefaultUnitVisual, DefaultUnitModel, UnitPath> defaultUnitFactory)
    {
        _defaultUnitVisualFactory = defaultUnitVisualFactory;
        _defaultUnitFactory = defaultUnitFactory;
    }

    public virtual UnitBase Spawn(DefaultUnitModel model, UnitPath path, Transform parent)
    {
        var pathMapper = new UnitPath.UnitPathMapper(path);
        var data = new DefaultUnitVisual.DefaultUnitVisualFactoryData(pathMapper.MapPoint(0.0f), Quaternion.LookRotation(pathMapper.MapDirection(0.0f)), parent);
        
        var unitVisual = _defaultUnitVisualFactory.Create(data);
        var unit = _defaultUnitFactory.Create(unitVisual, model, path);

        return unit;
    }
}
