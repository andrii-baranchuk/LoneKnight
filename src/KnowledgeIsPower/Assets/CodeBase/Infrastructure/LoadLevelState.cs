using CodeBase.CameraLogic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure
{
  public class LoadLevelState : IPayloadedState<string>
  {
    private const string InitialPointTag = "PlayerInitialPoint";
    private const string HeroPath = "Hero/hero";
    private const string HudPath = "Hud/Hud";
    private readonly GameStateMachine _stateMachine;
    private readonly SceneLoader _sceneLoader;
    private readonly LoadingCurtain _loadingCurtain;

    public LoadLevelState(GameStateMachine stateMachine, SceneLoader sceneLoader, LoadingCurtain loadingCurtain)
    {
      _stateMachine = stateMachine;
      _sceneLoader = sceneLoader;
      _loadingCurtain = loadingCurtain;
    }

    public void Enter(string sceneName)
    {
      _loadingCurtain.Show();
      _sceneLoader.Load(sceneName, OnLoaded);
    }

    public void Exit()
    {
      _loadingCurtain.Hide();
    }

    private void OnLoaded()
    {
      var initialPoint = GameObject.FindGameObjectWithTag(InitialPointTag);
      
      GameObject hero = Instantiate(HeroPath, initialPoint.transform.position);
      CameraFollow(hero);
      
      Instantiate(HudPath);
      
      _stateMachine.Enter<GameLoopState>();
    }
    
    private void CameraFollow(GameObject hero) =>
      Camera.main.GetComponent<CameraFollow>().Follow(hero);

    private static GameObject Instantiate(string path)
    {
      var prefab = Resources.Load<GameObject>(path);
      Debug.Log(prefab);
      return Object.Instantiate(prefab);
    }
    
    private static GameObject Instantiate(string path, Vector3 at)
    {
      var prefab = Resources.Load<GameObject>(path);
      Debug.Log(prefab);
      return Object.Instantiate(prefab, at, Quaternion.identity);
    }
  }
}