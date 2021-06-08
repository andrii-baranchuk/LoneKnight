using System;

namespace CodeBase.Data
{
  [Serializable]
  public class LootPieceData
  {
    public string Id;
    public Loot Loot;
    public Vector3Data Position;

    public LootPieceData(string id,Loot loot, Vector3Data position)
    {
      Id = id;
      Loot = loot;
      Position = position;
    }
  }
}