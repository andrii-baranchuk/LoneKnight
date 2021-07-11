using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Enemy;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic.EnemySpawners;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
  public interface IGameFactory : IService
  {
    List<ISavedProgressReader> ProgressReaders { get; }
    List<ISavedProgress> ProgressWriters { get; }

    Task<GameObject> CreateHero(Vector3 at);
    Task<GameObject> CreateHud();
    void CleanUp();
    Task CreateSpawner(Vector3 at, string spawnerId, MonsterTypeId monsterId);
    Task<GameObject> CreateMonster(MonsterTypeId typeId, Transform parent, SpawnPoint spawner);
    Task<LootPiece> CreateLoot();
    Task WarmUp();
  }
}