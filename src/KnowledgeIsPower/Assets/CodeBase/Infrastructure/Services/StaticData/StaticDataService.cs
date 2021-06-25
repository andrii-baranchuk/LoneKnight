using System.Collections.Generic;
using System.Linq;
using CodeBase.StaticData;
using CodeBase.StaticData.Windows;
using CodeBase.UI.Services.Windows;
using UnityEngine;

namespace CodeBase.Infrastructure.Services.StaticData
{
  public class StaticDataService : IStaticDataService
  {
    private const string StaticDataWindowsPath = "StaticData/UI/WindowData";
    private const string StaticDataLevelsPath = "StaticData/Levels";
    private const string StaticDataMonstersPath = "StaticData/Monsters";

    private Dictionary<MonsterTypeId,MonsterStaticData> _monsters;
    private Dictionary<string,LevelStaticData> _levels;
    private Dictionary<WindowId, WindowConfig> _windowConfigs;

    public void LoadMonsters()
    {
      _monsters = Resources.LoadAll<MonsterStaticData>(StaticDataMonstersPath)
        .ToDictionary(x => x.MonsterTypeId, x => x);
    }
    
    public void LoadLevels()
    {
      _levels = Resources.LoadAll<LevelStaticData>(StaticDataLevelsPath)
        .ToDictionary(x => x.LevelKey, x => x);
    }

    public void LoadWindows()
    {
      _windowConfigs = Resources.Load<WindowStaticData>(StaticDataWindowsPath)
        .WindowConfigs
        .ToDictionary(x => x.WindowId, x => x);
    }

    public MonsterStaticData ForMonster(MonsterTypeId typeId) => 
      _monsters.TryGetValue(typeId, out MonsterStaticData staticData) 
        ? staticData 
        : null;

    public LevelStaticData ForLevel(string sceneKey) =>
      _levels.TryGetValue(sceneKey, out LevelStaticData staticData) 
        ? staticData 
        : null;

    public WindowConfig ForWindow(WindowId windowId) =>
      _windowConfigs.TryGetValue(windowId, out WindowConfig windowConfig) 
        ? windowConfig 
        : null;
  }
}