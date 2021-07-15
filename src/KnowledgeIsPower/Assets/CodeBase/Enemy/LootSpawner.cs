using CodeBase.Data;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
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

    public void Construct(IGameFactory factory, IRandomService random)
    {
      _factory = factory;
      _random = random;
    }

    private void Start()
    {
      EnemyDeath.Happened += SpawnLoot;
    }

    private async void SpawnLoot()
    {
      Loot lootItem = GenerateLootItem(_lootMin, _lootMax, transform.position);
      LootPiece lootPiece = await _factory.CreateLoot();
      lootPiece.Initialize(lootItem);
    }
    
    private Loot GenerateLootItem(int lootMin, int lootMax, Vector3 position)
    {
      return new Loot
      {
        Value = _random.Next(lootMin, lootMax), 
        Position = position
      };
    }
    
    public void SetLoot(int min, int max)
    {
      _lootMin = min;
      _lootMax = max;
    }
  }
}