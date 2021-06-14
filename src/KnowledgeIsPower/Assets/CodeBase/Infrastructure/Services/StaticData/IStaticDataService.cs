using CodeBase.StaticData;

namespace CodeBase.Infrastructure.Services.StaticData
{
  public interface IStaticDataService : IService
  {
    void LoadMonsters();
    void LoadLevels();
    MonsterStaticData ForMonster(MonsterTypeId typeId);
    LevelStaticData ForLevel(string sceneKey);
  }
}