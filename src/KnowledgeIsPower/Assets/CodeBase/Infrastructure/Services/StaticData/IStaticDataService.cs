using CodeBase.StaticData;
using CodeBase.StaticData.Windows;
using CodeBase.UI.Services.Windows;

namespace CodeBase.Infrastructure.Services.StaticData
{
  public interface IStaticDataService : IService
  {
    void LoadMonsters();
    void LoadLevels();
    void LoadWindows();
    MonsterStaticData ForMonster(MonsterTypeId typeId);
    LevelStaticData ForLevel(string sceneKey);
    WindowConfig ForWindow(WindowId shop);
  }
}