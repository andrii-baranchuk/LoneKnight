using System.Collections;
using CodeBase.Data;
using CodeBase.Enemy;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Logic.EnemySpawners
{
  public class SpawnPoint : MonoBehaviour, ISavedProgress
  {
    public string Id { get; set; }
    public MonsterTypeId MonsterTypeId;
    public bool Slain;

    private IGameFactory _factory;
    private EnemyDeath _enemyDeath;

    public void Construct(IGameFactory factory) => _factory = factory;

    private void Spawn()
    {
      GameObject monster = _factory.CreateMonster(MonsterTypeId, transform, this);
      _enemyDeath = monster.GetComponent<EnemyDeath>();
      _enemyDeath.Happened += Slay;
    }

    private void Slay()
    {
      if (_enemyDeath != null) 
        _enemyDeath.Happened -= Slay;
      
      Slain = true;
    }

    private IEnumerator SpawnLoot(PlayerProgress progress)
    {
      yield return new WaitForSeconds(0.1f);
      
      var lootData = progress.WorldData.LootData.NotCollectedLoot.Find(x => x.Id == Id);
      if (lootData != null)
      {
        LootPiece loot = _factory.CreateLoot();
        loot.transform.position = lootData.Position.AsUnityVector();
        loot.Initialize(lootData.Loot, this);
      }
    }

    public void LoadProgress(PlayerProgress progress)
    {
      if (progress.KillData.ClearedSpawners.Contains(Id))
      {
        Slain = true;

        StartCoroutine(SpawnLoot(progress));
      }
      else
      {
        Spawn();
      }

    }

    public void UpdateProgress(PlayerProgress progress)
    {
      if (Slain)
        progress.KillData.ClearedSpawners.Add(Id);
    }
  }
}