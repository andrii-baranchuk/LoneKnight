using System;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using UnityEngine;

namespace CodeBase.Enemy
{
  public class RotateToPlayer : EnemyFollow
  {
    private float Speed = 1f;

    private IGameFactory _gameFactory;
    private Transform _heroTransform;
    private Vector3 _positionToLook;

    private void Start()
    {
      _gameFactory = AllServices.Container.Single<IGameFactory>();

      if (_gameFactory.HeroGameObject != null)
        InitializeHeroTransform();
      else
      {
        _gameFactory.HeroCreated += HeroCreated;
      }
    }

    private void Update()
    {
      if(HeroInitialized())
        RotateTowardsHero();
    }

    private void InitializeHeroTransform() =>
      _heroTransform = _gameFactory.HeroGameObject.transform;
    
    private void HeroCreated() =>
      InitializeHeroTransform();
    
    private bool HeroInitialized() => 
      _heroTransform != null;

    private void RotateTowardsHero()
    {
      UpdatePositionToLookAt();

      transform.rotation = SmoothRotation(transform.rotation, _positionToLook);
    }

    private void UpdatePositionToLookAt()
    {
      Vector3 positionDiff = _heroTransform.position - transform.position;
      _positionToLook = new Vector3(positionDiff.x, transform.position.y, positionDiff.z);
    }

    private Quaternion SmoothRotation(Quaternion rotation, Vector3 positionToLook)
    {
      return Quaternion.Lerp(rotation, TargetRotation(positionToLook), SpeedFactor());
    }

    private Quaternion TargetRotation(Vector3 position) => 
      Quaternion.LookRotation(position);

    private float SpeedFactor() => 
      Speed * Time.deltaTime;
  }
}