using System.Collections.Generic;
using System.Linq;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Infrastructure.Services.StaticData
{
  public class StaticDataService : IStaticDataService
  {
    private Dictionary<MonsterTypeId,MonsterStaticData> _monsters;
    private Dictionary<string,LevelStaticData> _levels;

    public void LoadMonsters()
    {
      _monsters = Resources.LoadAll<MonsterStaticData>("StaticData/Monsters")
        .ToDictionary(x => x.MonsterTypeId, x => x);
    }
    
    public void LoadLevels()
    {
      _levels = Resources.LoadAll<LevelStaticData>("StaticData/Levels")
        .ToDictionary(x => x.LevelKey, x => x);
    }

    public MonsterStaticData ForMonster(MonsterTypeId typeId) => 
      _monsters.TryGetValue(typeId, out MonsterStaticData staticData) 
        ? staticData 
        : null;

    public LevelStaticData ForLevel(string sceneKey) =>
      _levels.TryGetValue(sceneKey, out LevelStaticData staticData) 
        ? staticData 
        : null;
    
  }
}