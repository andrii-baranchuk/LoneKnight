using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Enemy;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.States;
using CodeBase.Logic;
using CodeBase.Logic.EnemySpawners;
using CodeBase.Logic.TransferTrigger;
using CodeBase.StaticData;
using CodeBase.UI.Elements;
using CodeBase.UI.Services.Windows;
using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Infrastructure.Factory
{
  public class GameFactory : IGameFactory
  {
    public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
    public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();
    private GameObject HeroGameObject { get; set; }

    private readonly IAssetProvider _assets;
    private readonly IStaticDataService _staticData;
    private readonly IRandomService _randomService;
    private readonly IPersistentProgressService _progressService;
    private readonly IWindowService _windowService;
    private readonly IGameStateMachine _stateMachine;

    public GameFactory(
      IAssetProvider assets, 
      IStaticDataService staticData, 
      IRandomService randomService, 
      IPersistentProgressService progressService, 
      IWindowService windowService,
      IGameStateMachine stateMachine)
    {
      _assets = assets;
      _staticData = staticData;
      _progressService = progressService;
      _windowService = windowService;
      _stateMachine = stateMachine;
      _randomService = randomService;
    }

    public async Task WarmUp()
    {
      await _assets.Load<GameObject>(AssetsAddress.Loot);
      await _assets.Load<GameObject>(AssetsAddress.Spawner);
    }

    public async Task<GameObject> CreateHero(Vector3 at)
    {
      HeroGameObject = await InstantiateRegistredAsync(AssetsAddress.HeroPath, at);
      return HeroGameObject;
    }

    public async Task<GameObject> CreateHud()
    {
      GameObject hud = await InstantiateRegistredAsync(AssetsAddress.HudPath);
      hud.GetComponentInChildren<LootCounter>().Construct(_progressService.Progress.WorldData);

      foreach (OpenWindowButton openWindowButton in hud.GetComponentsInChildren<OpenWindowButton>()) 
        openWindowButton.Construct(_windowService);

      return hud;
    }

    public async Task<GameObject> CreateMonster(MonsterTypeId typeId, Transform parent)
    {
      MonsterStaticData monsterData = _staticData.ForMonster(typeId);

      GameObject prefab = await _assets.Load<GameObject>(monsterData.PrefabReference);
      GameObject monster = Object.Instantiate(prefab, parent.position, Quaternion.identity, parent);
      
      IHealth health = monster.GetComponent<IHealth>();
      health.Current = monsterData.Hp;
      health.Max = monsterData.Hp;

      monster.GetComponent<ActorUI>().Construct(health);
      monster.GetComponent<AgentMoveToHero>().Construct(HeroGameObject.transform);
      monster.GetComponent<NavMeshAgent>().speed = monsterData.MoveSpeed;

      Attack attack = monster.GetComponent<Attack>();
      attack.Construct(HeroGameObject.transform);
      attack.Damage = monsterData.Damage;
      attack.Cleavage = monsterData.Cleavage;
      attack.EffectiveDistance = monsterData.EffectiveDistance;
      
      monster.GetComponent<RotateToHero>()?.Construct(HeroGameObject.transform);

      LootSpawner lootSpawner = monster.GetComponentInChildren<LootSpawner>();
      lootSpawner.SetLoot(monsterData.MinLoot, monsterData.MaxLoot);
      lootSpawner.Construct(this, _randomService);
      
      return monster;
    }

    public async Task<LootPiece> CreateLoot()
    {
      GameObject prefab = await _assets.Load<GameObject>(AssetsAddress.Loot);
      LootPiece lootPiece = InstantiateRegistred(prefab)
        .GetComponent<LootPiece>();
      
      lootPiece.Construct(_progressService.Progress.WorldData);
      
      return lootPiece;
    }

    public async Task CreateSpawner(Vector3 at, string spawnerId, MonsterTypeId monsterTypeId)
    {
      GameObject prefab = await _assets.Load<GameObject>(AssetsAddress.Spawner);
      
      SpawnPoint spawner = InstantiateRegistred(prefab, at)
        .GetComponent<SpawnPoint>();

      spawner.Construct(this);
      spawner.Id = spawnerId;
      spawner.MonsterTypeId = monsterTypeId;
    }

    public async Task CreateTransferTrigger(Vector3 transferPosition, Vector3 scale, string TransferTo)
    {
      GameObject prefab = await _assets.Load<GameObject>(AssetsAddress.TransferTrigger);
      
      GameObject transfer = Object.Instantiate(prefab, transferPosition, Quaternion.identity);
      transfer.transform.localScale = scale;
        
      LevelTransferTrigger transferTrigger = transfer.GetComponent<LevelTransferTrigger>();

      transferTrigger.Constuct(_stateMachine, TransferTo);

    }

    public void Register(ISavedProgressReader progressReader)
    {
      if (progressReader is ISavedProgress progressWriter) 
        ProgressWriters.Add(progressWriter);

      ProgressReaders.Add(progressReader);
    }

    public void Cleanup()
    {
      ProgressReaders.Clear();
      ProgressWriters.Clear();
      
      _assets.CleanUp();
    }

    private async Task<GameObject> InstantiateRegistredAsync(string prefabPath, Vector3 position)
    {
      GameObject heroGameObject = await _assets.Instantiate(prefabPath, position);
      RegisterProgressWatchers(heroGameObject);
      return heroGameObject;
    }

    private async Task<GameObject> InstantiateRegistredAsync(string prefabPath)
    {
      GameObject heroGameObject = await _assets.Instantiate(prefabPath);
      RegisterProgressWatchers(heroGameObject);
      return heroGameObject;
    }

    private GameObject InstantiateRegistred(GameObject prefab, Vector3 position)
    {
      GameObject heroGameObject = Object.Instantiate(prefab, position, Quaternion.identity);
      RegisterProgressWatchers(heroGameObject);
      return heroGameObject;
    }

    private GameObject InstantiateRegistred(GameObject prefab)
    {
      GameObject heroGameObject = Object.Instantiate(prefab);
      RegisterProgressWatchers(heroGameObject);
      return heroGameObject;
    }

    private void RegisterProgressWatchers(GameObject gameObject)
    {
      foreach (ISavedProgressReader progressReader in gameObject.GetComponentsInChildren<ISavedProgressReader>()) 
        Register(progressReader);
    }
  }
}