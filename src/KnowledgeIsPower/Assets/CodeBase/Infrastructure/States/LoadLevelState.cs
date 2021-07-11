using System.Threading.Tasks;
using CodeBase.CameraLogic;
using CodeBase.Hero;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.StaticData;
using CodeBase.Logic;
using CodeBase.StaticData;
using CodeBase.UI.Elements;
using CodeBase.UI.Services.Factory;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure.States
{
  public class LoadLevelState : IPayloadedState<string>
  {
    private const string EnemySpawnerTag = "EnemySpawner";
    private readonly GameStateMachine _stateMachine;
    private readonly SceneLoader _sceneLoader;
    private readonly LoadingCurtain _loadingCurtain;
    private readonly IGameFactory _gameFactory;
    private readonly IPersistentProgressService _progressService;
    private readonly IStaticDataService _staticData;
    private readonly IUIFactory _uiFactory;

    public LoadLevelState(GameStateMachine stateMachine, SceneLoader sceneLoader, LoadingCurtain loadingCurtain, IGameFactory gameFactory, IPersistentProgressService progressService, IStaticDataService staticData, IUIFactory uiFactory)
    {
      _stateMachine = stateMachine;
      _sceneLoader = sceneLoader;
      _loadingCurtain = loadingCurtain;
      _gameFactory = gameFactory;
      _progressService = progressService;
      _staticData = staticData;
      _uiFactory = uiFactory;
    }

    public void Enter(string sceneName)
    {
      _loadingCurtain.Show();
      _gameFactory.CleanUp();
      _gameFactory.WarmUp();
      _sceneLoader.Load(sceneName, OnLoaded);
    }

    public void Exit()
    {
      _loadingCurtain.Hide();
    }

    private async void OnLoaded()
    {
      await InitUIRoot();
      await IntiGameWorld();
      InformProgressReaders();
      
      _stateMachine.Enter<GameLoopState>();
    }

    private async Task InitUIRoot()
    {
      await _uiFactory.CreateUIRoot();
    }

    private async Task IntiGameWorld()
    {
      LevelStaticData levelData = LevelStaticData();


      await InitSpawners(levelData);
      GameObject hero = await InitHero(levelData);
      await InitHud(hero);
      
      CameraFollow(hero);
    }

    private async Task InitSpawners(LevelStaticData levelData)
    {
      foreach (var spawnerData in levelData.EnemySpawners)
      {
        await _gameFactory.CreateSpawner(spawnerData.Position, spawnerData.Id, spawnerData.MonsterTypeId);
      }
    }

    private async Task<GameObject> InitHero(LevelStaticData levelData) => 
      await _gameFactory.CreateHero(at: levelData.InitialLevelPosition);

    private async Task InitHud(GameObject hero)
    {
      GameObject hud = await _gameFactory.CreateHud();

      hud.GetComponentInChildren<ActorUI>()
        .Construct(hero.GetComponent<HeroHealth>());
    }

    private void CameraFollow(GameObject hero) =>
      Camera.main.GetComponent<CameraFollow>().Follow(hero);

    private void InformProgressReaders()
    {
      foreach (ISavedProgressReader progressReader in _gameFactory.ProgressReaders)
      {
        progressReader.LoadProgress(_progressService.Progress);
      }
    }

    private LevelStaticData LevelStaticData()
    {
      string sceneKey = SceneManager.GetActiveScene().name;
      LevelStaticData levelData = _staticData.ForLevel(sceneKey);
      return levelData;
    }
  }
}