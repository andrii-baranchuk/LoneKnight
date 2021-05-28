using CodeBase.StaticData;

namespace CodeBase.Infrastructure.Services.StaticData
{
  public interface IStaticDataService : IService
  {
    void LoadMonsters();
    MonsterStaticData ForMonster(MonsterTypeId typeId);
  }
}