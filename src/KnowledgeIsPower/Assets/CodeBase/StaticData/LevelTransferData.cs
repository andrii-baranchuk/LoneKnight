using System;
using UnityEngine;

namespace CodeBase.StaticData
{
  [Serializable]
  public class LevelTransferData 
  {
    public Vector3 Position;
    public Vector3 Scale;
    public string TransferTo;

    public LevelTransferData(Vector3 position, Vector3 scale, string at)
    {
      Position = position;
      Scale = scale;
      TransferTo = at;
    }
  }
}