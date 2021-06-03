using System.Collections.Generic;
using CodeBase.Hero;
using CodeBase.Infrastructure.Services;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
  public interface IGameFactory : IService
  {
    GameObject CreateHero(GameObject at);
    GameObject CreateHud();
    List<ISavedProgressReader> ProgressReaders { get; }
    List<ISavedProgress> ProgressWriters { get; }
    void Register(ISavedProgressReader progressReader);
    void Cleanup();
    GameObject CreateMonster(MonsterTypeId typeId, Transform parent);
  }
}