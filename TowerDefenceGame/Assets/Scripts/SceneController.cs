using Core;
using Installers;
using Settings;
using UnityEngine;

[DisallowMultipleComponent]
public sealed class SceneController : MonoBehaviour
{
    private readonly UnitsController _unitsController = UnitsController.Instance;
    private readonly TowersController _towersController = TowersController.Instance;

    [SerializeField]
    private SceneInstaller _sceneInstaller;
    
    public static SceneController Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<SceneController>();

            return _instance;
        }
    }
    
    private static SceneController _instance;
    
    private void Start()
    {
        if(_sceneInstaller != null)
            _sceneInstaller.Install();

        if (_sceneInstaller.Base != null)
        {
            _sceneInstaller.Base.Destroyed += OnBaseDestroyed;
        }
        
        StartGame();
    }

    private void OnBaseDestroyed(IAttackTarget obj)
    {
        EndGame();
    }

    public void StartGame()
    {
        if (_sceneInstaller.UnitWavesController != null)
        {
            _sceneInstaller.UnitWavesController.StartSpawn();
        }

        if (_unitsController != null)
        {
            _unitsController.StartUpdateUnits();
        }

        if (_towersController != null)
        {
            _towersController.StartUpdateTowers();
        }
    }

    public void StopGame()
    {
        if (_sceneInstaller.UnitWavesController != null)
        {
            _sceneInstaller.UnitWavesController.StopSpawn();
        }
        
        if (_unitsController != null)
        {
            _unitsController.StopUpdateUnits();
        }
        
        if (_towersController != null)
        {
            _towersController.StopUpdateTowers();
        }
    }

    public void EndGame()
    {
        StopGame();
    }
}
