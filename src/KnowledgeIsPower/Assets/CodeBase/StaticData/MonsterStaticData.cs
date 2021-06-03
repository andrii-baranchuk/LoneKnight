using System;
using UnityEngine;

namespace CodeBase.StaticData
{
  [CreateAssetMenu(fileName = "MonsterData", menuName = "StaticData/Monster", order = 0)]
  public class MonsterStaticData : ScriptableObject
  {
    public MonsterTypeId MonsterTypeId;

    [Range(1, 100)] 
    public int Hp = 50;

    [Range(1, 30)] 
    public float Damage = 15;

    [Range(0.5f, 1)] 
    public float EffectiveDistance = 0.75f;

    [Range(0.5f, 1)] 
    public float Cleavage = 0.75f;
    
    [Range(1,10)]
    public float MoveSpeed = 5;
    
    public GameObject Prefab;
  }
}