using System;
using System.Collections.Generic;

namespace CodeBase.Data
{
  [Serializable]
  public class LootData
  {
    public int Collected;
    public event Action Changed;

    public List<LootPieceData> NotCollectedLoot = new List<LootPieceData>();

    public void Collect(Loot loot)
    {
      Collected += loot.Value;
      Changed?.Invoke();
    }
    
    public void Add(int loot)
    {
      Collected += loot;
      Changed?.Invoke();
    }
  }
}