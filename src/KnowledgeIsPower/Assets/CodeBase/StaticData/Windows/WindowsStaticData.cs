using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.StaticData.Windows
{
  [CreateAssetMenu(fileName = "WindowStaticData", menuName = "StaticData/Window")]
  public class WindowsStaticData : ScriptableObject
  {
    public List<WindowConfig> Configs;
  }
}