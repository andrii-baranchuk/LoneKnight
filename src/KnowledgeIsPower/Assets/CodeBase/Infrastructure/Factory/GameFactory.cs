using System.Collections.Generic;
using CodeBase.Enemy;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using CodeBase.Logic.EnemySpawners;
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

    public GameFactory(IAssetProvider assets, IStaticDataService staticData, IRandomService randomService, IPersistentProgressService progressService, IWindowService windowService)
    {
      _assets = assets;
      _staticData = staticData;
      _progressService = progressService;
      _windowService = windowService;
      _randomService = randomService;
    }
    public GameObject CreateHero(GameObject at)
    {
      HeroGameObject = InstantiateRegistred(AssetPath.HeroPath, at.transform.position);
      return HeroGameObject;
    }

    public GameObject CreateHud()
    {
      GameObject hud = InstantiateRegistred(AssetPath.HudPath);
      hud.GetComponentInChildren<LootCounter>().Construct(_progressService.Progress.WorldData);

      foreach (OpenWindowButton openWindowButton in hud.GetComponentsInChildren<OpenWindowButton>()) 
        openWindowButton.Construct(_windowService);

      return hud;
    }

    public GameObject CreateMonster(MonsterTypeId typeId, Transform parent)
    {
      MonsterStaticData monsterData = _staticData.ForMonster(typeId);
      GameObject monster = Object.Instantiate(monsterData.Prefab, parent.position, Quaternion.identity, parent);
      
      var health = monster.GetComponent<IHealth>();
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

      var lootSpawner = monster.GetComponentInChildren<LootSpawner>();
      lootSpawner.SetLoot(monsterData.MinLoot, monsterData.MaxLoot);
      lootSpawner.Construct(this, _randomService);
      
      return monster;
    }

    public LootPiece CreateLoot()
    {
      LootPiece lootPiece = InstantiateRegistred(AssetPath.Loot).GetComponent<LootPiece>();
      lootPiece.Construct(_progressService.Progress.WorldData);
      return lootPiece;
    }

    public void CreateSpawner(Vector3 at, string spawnerId, MonsterTypeId monsterTypeId)
    {
      SpawnPoint spawner = InstantiateRegistred(AssetPath.Spawner, at)
        .GetComponent<SpawnPoint>();

      spawner.Construct(this);
      spawner.Id = spawnerId;
      spawner.MonsterTypeId = monsterTypeId;
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
    }

    private GameObject InstantiateRegistred(string prefabPath, Vector3 position)
    {
      GameObject heroGameObject = _assets.Instantiate(prefabPath, position);
      RegisterProgressWatchers(heroGameObject);
      return heroGameObject;
    }

    private GameObject InstantiateRegistred(string prefabPath)
    {
      GameObject heroGameObject = _assets.Instantiate(prefabPath);
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