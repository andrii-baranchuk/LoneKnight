using System;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Enemy
{
  public class AgentMoveToHero : Follow
  {
    private const float MinimalDistance = 1f;
    public NavMeshAgent Agent;
    private Transform _heroTransform;

    private void Start()
    {
      IGameFactory gameFactory = AllServices.Container.Single<IGameFactory>();

      if (gameFactory.HeroGameObject != null)
        InitializeHeroTransform(gameFactory.HeroGameObject);
      else
        gameFactory.HeroCreated += InitializeHeroTransform;
    }

    private void Update()
    {
      if(HasHeroTransform() &&HeroNotReached())
        Agent.destination = _heroTransform.position;
    }

    private bool HasHeroTransform() => 
      _heroTransform != null;

    private bool HeroNotReached() => 
      Vector3.Distance(Agent.transform.position, _heroTransform.position) >= MinimalDistance;

    private void InitializeHeroTransform(GameObject heroGameObject) => 
      _heroTransform = heroGameObject.transform;
  }
}