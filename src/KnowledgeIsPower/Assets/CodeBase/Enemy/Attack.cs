﻿using System;
using System.Linq;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using UnityEngine;

namespace CodeBase.Enemy
{
  [RequireComponent(typeof(EnemyAnimator))]
  public class Attack : MonoBehaviour
  {
    public EnemyAnimator Animator;

    public float AttackCooldown = 3f;
    public float Cleavage = 1f;
    public float EffectiveDistance = 1.0f;

    private IGameFactory _factory;
    private Transform _heroTransform;
    private float _attackCooldown;
    private bool _isAttacking;
    private Collider[] _hits = new Collider[1];
    private int _layerMask;
    private bool _attackIsActive;

    private void Awake()
    {
      _factory = AllServices.Container.Single<IGameFactory>();

      _layerMask = 1 << LayerMask.NameToLayer("Player");
      _factory.HeroCreated += OnHeroCreated;
    }

    private void Update()
    {
      UpdateCooldown();

      if (CanAttack())
        StartAttack();
    }

    //Invoked from Unity Animator
    private void OnAttack()
    {
      if (Hit(out Collider hit))
      {
        PhysicsDebug.DrawSphere(StartPoint(), Cleavage, 1f);
      }
    }
    
    //Invoked from Unity Animator
    private void OnAttackEnded()
    {
      _attackCooldown = AttackCooldown;
      _isAttacking = false;
    }
    
    private void StartAttack()
    {
      transform.LookAt(_heroTransform);
      Animator.PlayAttack();

      _isAttacking = true;
    }

    public void EnableAttack() => 
      _attackIsActive = true;

    public void DisableAttack() => 
      _attackIsActive = false;

    private bool Hit(out Collider hit)
    {
      int hitCount = Physics.OverlapSphereNonAlloc(StartPoint(), Cleavage, _hits, _layerMask);

      hit = _hits.FirstOrDefault();
      
      return hitCount > 0;
    }

    private Vector3 StartPoint()
    {
      return new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z)
             + transform.forward * EffectiveDistance;
    }
    
    private bool CooldownIsUp()
    {
      return _attackCooldown <= 0f;
    }

    private void UpdateCooldown()
    {
      if (!CooldownIsUp())
        _attackCooldown -= Time.deltaTime;
    }

    private bool CanAttack() => 
      _attackIsActive && CooldownIsUp() && !_isAttacking;

    private void OnHeroCreated() =>
      _heroTransform = _factory.HeroGameObject.transform;
  }
}