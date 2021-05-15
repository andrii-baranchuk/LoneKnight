using System;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Enemy
{
  public class AgentMoveToPlayer : EnemyFollow
  {
    private const float MinimalDistance = 1f;

    public NavMeshAgent Agent;

    private IGameFactory _gameFactory;
    private Transform _heroTransform;

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
      if (HeroInitialized() && HeroNotReached())
        Agent.destination = _heroTransform.position;
    }

    private bool HeroInitialized() => 
      _heroTransform != null;

    private void InitializeHeroTransform() =>
      _heroTransform = _gameFactory.HeroGameObject.transform;

    private void HeroCreated() =>
      InitializeHeroTransform();

    private bool HeroNotReached() =>
      Vector3.Distance(Agent.transform.position, _heroTransform.position) >= MinimalDistance;
  }
}