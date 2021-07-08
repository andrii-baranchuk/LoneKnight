using System;

namespace CodeBase.Data
{
  [Serializable]
  public class LootData
  {
    public int Collected;
    public Action Changed;
    public UnpickedLoot UnpickedLoot;

    public LootData()
    {
      UnpickedLoot = new UnpickedLoot();
    }

    public void Collect(Loot loot)
    {
      Collected += loot.Value;
      Changed?.Invoke();
      UnpickedLoot.Loot.Remove(loot);
    }    
    public void Add(int loot)
    {
      Collected += loot;
      Changed?.Invoke();
    }
  }
}