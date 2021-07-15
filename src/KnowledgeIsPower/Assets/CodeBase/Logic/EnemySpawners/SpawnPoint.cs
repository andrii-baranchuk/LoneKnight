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
    public MonsterTypeId MonsterTypeId;
    public bool Slain;
    public string Id { get; set; }

    private IGameFactory _factory;
    private EnemyDeath enemyDeath;

    public void Construct(IGameFactory factory) => 
      _factory = factory;

    public void LoadProgress(PlayerProgress progress)
    {
      if (progress.KillData.ClearedSpawners.Contains(Id))
        Slain = true;
      else
        Spawn();
    }

    public void UpdateProgress(PlayerProgress progress)
    {
      if (Slain) 
        progress.KillData.ClearedSpawners.Add(Id);
    }

    private async void Spawn()
    {
      GameObject monster = await _factory.CreateMonster(MonsterTypeId, transform);
      enemyDeath = monster.GetComponent<EnemyDeath>();
      enemyDeath.Happened += Slay;
    }

    private void Slay()
    {
      if (enemyDeath!=null) 
        enemyDeath.Happened -= Slay;
      
      Slain = true;
    }
  }
}