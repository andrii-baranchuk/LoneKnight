using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CodeBase.StaticData
{
  [CreateAssetMenu(fileName = "MonsterData", menuName = "StaticData/Monster")]
  public class MonsterStaticData : ScriptableObject
  {
    public MonsterTypeId MonsterTypeId;
    
    [Range(1, 100)]
    public int Hp;
    
    [Range(1,30)]
    public float Damage;

    [Range(1, 30)] 
    public float MoveSpeed;

    [Range(0.5f, 1f)]
    public float EffectiveDistance;

    [Range(0.5f, 1f)]
    public float Cleavage;

    public int MinLoot;
    public int MaxLoot;

    public AssetReferenceGameObject PrefabReference;
  }
}