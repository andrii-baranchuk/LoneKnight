using System.Collections.Generic;
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

    GameObject CreateHero(GameObject at);
    GameObject CreateHud();
    void CleanUp();
    void CreateSpawner(Vector3 at, string spawnerId, MonsterTypeId monsterId);
    GameObject CreateMonster(MonsterTypeId typeId, Transform parent, SpawnPoint spawner);
    LootPiece CreateLoot();
  }
}