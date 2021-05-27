using System;
using System.Collections.Generic;
using CodeBase.Hero;
using CodeBase.Infrastructure.AssetManagement;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
  public class GameFactory : IGameFactory
  {
    public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
    public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();
    public GameObject HeroGameObject { get; private set; }
    public event Action<GameObject> HeroCreated;

    private readonly IAssetProvider _assets;
    public GameFactory(IAssetProvider assets)
    {
      _assets = assets;
    }
    public GameObject CreateHero(GameObject at)
    {
      HeroGameObject = InstantiateRegistred(AssetPath.HeroPath, at.transform.position);
      HeroCreated?.Invoke(HeroGameObject);
      return HeroGameObject;
    }

    public GameObject CreateHud() => 
      InstantiateRegistred(AssetPath.HudPath);

    public void Register(ISavedProgressReader progressReader)
    {
      if (progressReader is ISavedProgress progressWriter)
      {
        ProgressWriters.Add(progressWriter);
      }
      
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
      {
        Register(progressReader);
      }
    }
  }
}