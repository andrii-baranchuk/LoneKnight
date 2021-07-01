using System.Collections.Generic;
using System.Linq;
using CodeBase.StaticData.Windows;
using CodeBase.UI.Services.Windows;
using UnityEngine;

namespace CodeBase.StaticData
{
  public class StaticDataService : IStaticDataService
  {
    private const string StaticDataMonstersPath = "StaticData/Monsters";
    private const string StaticDataLevelsPath = "StaticData/Levels";
    private const string StaticDataWindowsPath = "StaticData/Windows/WindowStaticData";

    private Dictionary<MonsterTypeId, MonsterStaticData> _monsters;
    private Dictionary<string, LevelStaticData> _levels;
    private Dictionary<WindowId, WindowConfig> _windows;

    public void LoadMonsters()
    {
      _monsters = Resources
        .LoadAll<MonsterStaticData>(StaticDataMonstersPath)
        .ToDictionary(x => x.MonsterTypeId, x => x);

      _levels = Resources
        .LoadAll<LevelStaticData>(StaticDataLevelsPath)
        .ToDictionary(x => x.LevelKey, x => x);

      _windows = Resources
        .Load<WindowsStaticData>(StaticDataWindowsPath)
        .Configs
        .ToDictionary(x => x.WindowId, x => x);
    }

    public MonsterStaticData ForMonster(MonsterTypeId typeId) =>
      _monsters.TryGetValue(typeId,out MonsterStaticData staticData) 
        ? staticData 
        : null;

    public LevelStaticData ForLevel(string sceneKey) =>
      _levels.TryGetValue(sceneKey,out LevelStaticData staticData) 
        ? staticData 
        : null;

    public WindowConfig ForWindow(WindowId windowId) =>
      _windows.TryGetValue(windowId, out WindowConfig config)
        ? config
        : null;
  }
}