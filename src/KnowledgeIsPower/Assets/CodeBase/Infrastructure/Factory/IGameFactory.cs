using System;
using System.Collections.Generic;
using CodeBase.Hero;
using CodeBase.Infrastructure.Services;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
  public interface IGameFactory : IService
  {
    GameObject CreateHero(GameObject at);
    GameObject CreateHud();
    List<ISavedProgressReader> ProgressReaders { get; }
    List<ISavedProgress> ProgressWriters { get; }
    GameObject HeroGameObject { get; }
    event Action<GameObject> HeroCreated;
    void Cleanup();
  }
}