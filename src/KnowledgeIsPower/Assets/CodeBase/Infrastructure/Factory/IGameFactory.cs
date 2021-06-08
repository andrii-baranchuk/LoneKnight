using System;
using System.Collections.Generic;
using CodeBase.Enemy;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
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
    void Register(ISavedProgressReader progressReader);
    GameObject CreateMonster(MonsterTypeId typeId, Transform parent, EnemySpawner spawner);
    LootPiece CreateLoot();
  }
}