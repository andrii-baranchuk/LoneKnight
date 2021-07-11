using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Enemy;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.Random;
using CodeBase.Infrastructure.Services.StaticData;
using CodeBase.Logic;
using CodeBase.Logic.EnemySpawners;
using CodeBase.StaticData;
using CodeBase.UI.Elements;
using CodeBase.UI.Services.Windows;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AI;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CodeBase.Infrastructure.Factory
{
  public class GameFactory : IGameFactory
  {
    private readonly IAssets _assets;
    private readonly IStaticDataService _staticData;
    private readonly IRandomService _randomService;
    private readonly IPersistentProgressService _progressService;
    private readonly IWindowsService _windowService;

    public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
    public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();

    private GameObject heroGameObject { get; set; }

    public GameFactory(IAssets assets, IStaticDataService staticData,
      IRandomService randomService, IPersistentProgressService progressService,
      IWindowsService windowService)
    {
      _assets = assets;
      _staticData = staticData;
      _randomService = randomService;
      _progressService = progressService;
      _windowService = windowService;
    }

    public async Task WarmUp()
    {
      await _assets.Load<GameObject>(AssetAddress.Loot);
      await _assets.Load<GameObject>(AssetAddress.Spawner);
    }

    public async Task<GameObject> CreateHero(Vector3 at)
    {
      heroGameObject = await InstantiateRegistered(AssetAddress.HeroPath, at);

      return heroGameObject;
    }


    public async Task<GameObject> CreateMonster(MonsterTypeId typeId, Transform parent, SpawnPoint spawner)
    {
      MonsterStaticData monsterData = _staticData.ForMonster(typeId);

      GameObject prefab = await _assets.Load<GameObject>(monsterData.PrefabReference);
      GameObject monster = Object.Instantiate(prefab, parent.position, Quaternion.identity, parent);

      var health = monster.GetComponent<IHealth>();
      health.Current = monsterData.Hp;
      health.Max = monsterData.Hp;

      monster.GetComponent<ActorUI>().Construct(health);
      monster.GetComponent<AgentMoveToPlayer>().Construct(heroGameObject.transform);
      monster.GetComponent<NavMeshAgent>().speed = monsterData.MoveSpeed;

      var lootSpawner = monster.GetComponentInChildren<LootSpawner>();
      lootSpawner.SetLoot(monsterData.MinLoot, monsterData.MaxLoot);
      lootSpawner.Construct(this, _randomService, spawner);

      var attack = monster.GetComponent<EnemyAttack>();
      attack.Construct(heroGameObject.transform);
      attack.Damage = monsterData.Damage;
      attack.Cleavage = monsterData.Cleavage;
      attack.EffectiveDistance = monsterData.EffectiveDistance;

      monster.GetComponent<RotateToPlayer>()?.Construct(heroGameObject.transform);

      return monster;
    }

    public async Task<LootPiece> CreateLoot()
    {
      GameObject prefab = await _assets.Load<GameObject>(AssetAddress.Loot);
      
      LootPiece lootPiece = InstantiateRegistered(prefab).GetComponent<LootPiece>();
      lootPiece.Construct(_progressService.Progress.WorldData);

      return lootPiece;
    }

    public async Task CreateSpawner(Vector3 at, string spawnerId, MonsterTypeId monsterId)
    {
      GameObject prefab = await _assets.Load<GameObject>(AssetAddress.Spawner);
      
      SpawnPoint spawner = InstantiateRegistered(prefab, at)
        .GetComponent<SpawnPoint>();
      
      spawner.Construct(this);
      spawner.Id = spawnerId;
      spawner.MonsterTypeId = monsterId;
    }

    public async Task<GameObject> CreateHud()
    {
      GameObject hud = await InstantiateRegisteredAsync(AssetAddress.HudPath);

      hud.GetComponentInChildren<LootCounter>()
        .Construct(_progressService.Progress.WorldData);

      foreach (OpenWindowButton openWindowButton in hud.GetComponentsInChildren<OpenWindowButton>())
      {
        openWindowButton.Construct(_windowService);
      }

      return hud;
    }

    public void CleanUp()
    {
      ProgressReaders.Clear();
      ProgressWriters.Clear();

      _assets.CleanUp();
    }

    private async Task<GameObject> InstantiateRegistered(string prefabPath, Vector3 at)
    {
      GameObject gameObject = await _assets.Instantiate(prefabPath, at);
      RegisterProgressWatchers(gameObject);
      return gameObject;
    }

    private async Task<GameObject> InstantiateRegisteredAsync(string prefabPath)
    {
      GameObject gameObject = await _assets.Instantiate(prefabPath);
      RegisterProgressWatchers(gameObject);
      return gameObject;
    }

    private GameObject InstantiateRegistered(GameObject prefab, Vector3 at)
    {
      GameObject gameObject = Object.Instantiate(prefab,at, Quaternion.identity);
      RegisterProgressWatchers(gameObject);
      return gameObject;
    }

    private GameObject InstantiateRegistered(GameObject prefab)
    {
      GameObject gameObject = Object.Instantiate(prefab);
      RegisterProgressWatchers(gameObject);
      return gameObject;
    }

    private void RegisterProgressWatchers(GameObject gameObject)
    {
      foreach (var progressReader in gameObject.GetComponentsInChildren<ISavedProgressReader>())
      {
        Register(progressReader);
      }
    }

    public void Register(ISavedProgressReader progressReader)
    {
      if (progressReader is ISavedProgress progressWriter)
        ProgressWriters.Add(progressWriter);

      ProgressReaders.Add(progressReader);
    }
  }
}