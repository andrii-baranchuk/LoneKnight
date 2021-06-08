using System;
using CodeBase.Data;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.Random;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Enemy
{
  public class LootSpawner : MonoBehaviour
  {
    public EnemyDeath EnemyDeath;
    private IGameFactory _factory;
    private int _lootMin;
    private int _lootMax;
    private IRandomService _random;
    private EnemySpawner _spawner;

    public void Construct(IGameFactory factory, IRandomService random, EnemySpawner spawner)
    {
      _factory = factory;
      _random = random;
      _spawner = spawner;
    }

    private void Start()
    {
      EnemyDeath.Happened += SpawnLoot;
    }

    private void SpawnLoot()
    {
      LootPiece loot = _factory.CreateLoot();
      loot.transform.position = transform.position;

      Loot lootItem = GenerateLoot();
      loot.Initialize(lootItem, _spawner);
    }

    private Loot GenerateLoot()
    {
      return new Loot()
      {
        Value = _random.Next(_lootMin, _lootMax)
      };
    }

    public void SetLoot(int min, int max)
    {
      _lootMin = min;
      _lootMax = max;
    }
  }
}