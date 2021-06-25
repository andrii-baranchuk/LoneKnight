using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.StaticData.Windows
{
  [CreateAssetMenu(fileName = "WindowData", menuName = "StaticData/WindowData", order = 0)]
  public class WindowStaticData : ScriptableObject
  {
    public List<WindowConfig> WindowConfigs;
  }
}