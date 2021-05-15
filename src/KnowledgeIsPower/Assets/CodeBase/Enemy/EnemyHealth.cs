﻿using System;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Enemy
{
  [RequireComponent(typeof(EnemyAnimator))]
  public class EnemyHealth : MonoBehaviour, IHealth
  {
    public EnemyAnimator Animator;

    [SerializeField] 
    private float _current;
    [SerializeField] 
    private float _max;

    public event Action Changed;

    public float Current
    {
      get => _current;
      set => _current = value;
    }

    public float Max
    {
      get => _max;
      set => _max = value;
    }

    public void TakeDamage(float damage)
    {
      Current -= damage;
      Animator.PlayHit();
      
      Changed?.Invoke();
    }
  }
}