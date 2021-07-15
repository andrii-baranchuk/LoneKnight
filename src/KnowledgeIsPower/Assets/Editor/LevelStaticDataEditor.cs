using System.Linq;
using CodeBase.Logic;
using CodeBase.Logic.EnemySpawners;
using CodeBase.Logic.TransferTrigger;
using CodeBase.StaticData;
using UnityEditor;
using UnityEditor.Experimental.TerrainAPI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Editor
{
  [CustomEditor(typeof(LevelStaticData))]
  public class LevelStaticDataEditor : UnityEditor.Editor
  {
    private const string InitialPointTag = "InitialPoint";
    
    public override void OnInspectorGUI()
    {
      base.OnInspectorGUI();

      LevelStaticData levelData = (LevelStaticData) target;

      if(GUILayout.Button("Collect"))
      {
        levelData.EnemySpawners =
          FindObjectsOfType<SpawnMarker>()
            .Select(x => new EnemySpawnerData(x.GetComponent<UniqueId>().Id, x.MonsterTypeId, x.transform.position))
            .ToList();

        levelData.LevelTransfers =
          FindObjectsOfType<TransferMarker>()
            .Select(x => new LevelTransferData(x.transform.position, x.transform.localScale, x.TransferTo))
            .ToList();
          
        
        levelData.LevelKey = SceneManager.GetActiveScene().name;
        
        levelData.InitialHeroPosition = GameObject.FindWithTag(InitialPointTag).transform.position;
      }
      
      EditorUtility.SetDirty(target);
    }
  }
}