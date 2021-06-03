using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Enemy
{
  public class AgentMoveToHero : Follow
  {
    private const float MinimalDistance = 1f;

    public NavMeshAgent Agent;

    private Transform _heroTransform;

    public void Construct(Transform heroTransform) =>
      _heroTransform = heroTransform;

    private void Update() => 
      SetDestinationForAgent();

    private void SetDestinationForAgent()
    {
      if (HeroNotReached())
        Agent.destination = _heroTransform.position;
    }

    private bool HeroNotReached() =>
      Vector3.Distance(Agent.transform.position, _heroTransform.position) >= MinimalDistance;
  }
}