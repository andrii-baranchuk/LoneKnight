using CodeBase.CameraLogic;
using CodeBase.Logic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CodeBase.Infrastructure
{
  public class LoadLevelState : IPayloadedState<string>
  {
    private const string InitialPointTag = "InitialPoint";
    private const string HeroPath = "Hero/hero";
    private const string HudPath = "Hud/Hud";
    private readonly GameStateMachine _gameStateMachine;
    private readonly SceneLoader _sceneLoader;
    private readonly LoadingCurtain _curtain;

    public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, LoadingCurtain curtain)
    {
      _gameStateMachine = gameStateMachine;
      _sceneLoader = sceneLoader;
      _curtain = curtain;
    }

    public void Enter(string sceneName)
    {
      _curtain.Show();
      _sceneLoader.Load(sceneName, OnLoaded);
    }
    
    public void Exit() => 
      _curtain.Hide();

    private void OnLoaded()
    {
      GameObject initialPoint = GameObject.FindWithTag(InitialPointTag);
      
      GameObject hero = Instantiate(
        HeroPath,
        initialPoint.transform.position
        );
      Instantiate(HudPath);
      
      CameraFollow(hero);
      
      _gameStateMachine.Enter<GameLoopState>();
    }

    private static GameObject Instantiate(string path)
    {
      GameObject heroPrefab = Resources.Load<GameObject>(path);
      return Object.Instantiate(heroPrefab);
    }
    
    private static GameObject Instantiate(string path, Vector3 spawnPoint)
    {
      GameObject heroPrefab = Resources.Load<GameObject>(path);
      return Object.Instantiate(heroPrefab, spawnPoint, Quaternion.identity);
    }
    
    private void CameraFollow(GameObject hero)
    {
      Camera.main
        .GetComponent<CameraFollow>()
        .Follow(hero);
    }
  }
}