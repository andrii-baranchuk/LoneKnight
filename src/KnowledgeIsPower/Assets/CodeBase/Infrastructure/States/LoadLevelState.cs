using CodeBase.CameraLogic;
using CodeBase.Hero;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using CodeBase.UI;
using UnityEngine;

namespace CodeBase.Infrastructure.States
{
  public class LoadLevelState : IPayloadedState<string>
  {
    private const string InitialPointTag = "InitialPoint";
    private const string EnemySpawnerTag = "EnemySpawner";


    private readonly GameStateMachine _gameStateMachine;
    private readonly SceneLoader _sceneLoader;
    private readonly LoadingCurtain _curtain;
    private readonly IGameFactory _gameFactory;
    private readonly IPersistentProgressService _progressService;

    public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, LoadingCurtain curtain, 
      IGameFactory gameFactory, IPersistentProgressService progressService)
    {
      _gameStateMachine = gameStateMachine;
      _sceneLoader = sceneLoader;
      _curtain = curtain;
      _gameFactory = gameFactory;
      _progressService = progressService;
    }

    public void Enter(string sceneName)
    {
      _curtain.Show();
      _gameFactory.Cleanup();
      _sceneLoader.Load(sceneName, OnLoaded);
    }
    
    public void Exit() => 
      _curtain.Hide();

    private void OnLoaded()
    {
      InitGameWorld();
      InformProgressReaders();
      
      _gameStateMachine.Enter<GameLoopState>();
    }

    private void InformProgressReaders()
    {
      foreach (ISavedProgressReader progressReader in _gameFactory.ProgressReaders)
      {
        progressReader.LoadProgress(_progressService.Progress);
      }
    }

    private void InitGameWorld()
    {
      InitSpawners();
      
      GameObject initialPoint = GameObject.FindWithTag(InitialPointTag);

      GameObject hero = _gameFactory.CreateHero(initialPoint);

      InitHud(hero);

      CameraFollow(hero);
    }
    
    private void InitSpawners()
    {
      foreach (GameObject spawnerObject in GameObject.FindGameObjectsWithTag(EnemySpawnerTag))
      {
        EnemySpawner spawner = spawnerObject.GetComponent<EnemySpawner>();
        _gameFactory.Register(spawner);
      }
    }

    private void InitHud(GameObject hero)
    {
      GameObject hud = _gameFactory.CreateHud();
      hud.GetComponentInChildren<ActorUI>().Construct(hero.GetComponent<HeroHealth>());
    }

    private void CameraFollow(GameObject hero)
    {
      Camera.main
        .GetComponent<CameraFollow>()
        .Follow(hero);
    }
  }
}