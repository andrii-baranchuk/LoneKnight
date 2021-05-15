using System;
using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Hero
{
  [RequireComponent(typeof(HeroAnimator))]
  public class HeroHealth : MonoBehaviour, ISavedProgress, IHealth
  {
    public HeroAnimator Animator;
    private State _state;

    public event Action Changed;

    public float Current
    {
      get => _state.MaxHP;
      set
      {
        if (_state.CurrentHP != value)
        {
          _state.MaxHP = value;
          Changed?.Invoke();
        }
      }
    }

    public float Max
    {
      get => _state.CurrentHP;
      set => _state.CurrentHP = value;
    }


    public void LoadProgress(PlayerProgress progress)
    {
      _state = progress.HeroState;
      Changed?.Invoke();
    }

    public void UpdateProgress(PlayerProgress progress)
    {
      progress.HeroState.CurrentHP = Current;
      progress.HeroState.MaxHP = Max;
    }

    public void TakeDamage(float damage)
    {
      if (Current <= 0)
        return;

      Current -= damage;
      Animator.PlayHit();
    }
  }
}