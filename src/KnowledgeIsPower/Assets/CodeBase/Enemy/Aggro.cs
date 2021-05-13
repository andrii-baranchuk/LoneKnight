using System.Collections;
using UnityEngine;

namespace CodeBase.Enemy
{
  public class Aggro : MonoBehaviour
  {
    public TriggerObserver TriggerObserver;
    public Follow Follow;

    public float Cooldown;
    
    private Coroutine _aggroCorountine;
    private bool _hasAggroTarget;

    private void Start()
    {
      TriggerObserver.TriggerEnter += TriggerEnter;
      TriggerObserver.TriggerExit += TriggerExit;

      SwitchFollowOff();
    }

    private void TriggerEnter(Collider obj)
    {
      if (!_hasAggroTarget)
      {
        _hasAggroTarget = true;
        StopAggroCoroutine();
        SwitchFollowOn();
      }
    }

    private void TriggerExit(Collider obj)
    {
      if (_hasAggroTarget)
      {
        _hasAggroTarget = false;
        _aggroCorountine = StartCoroutine(SwithcFollowOffAfterCooldown());
      }
    }

    private IEnumerator SwithcFollowOffAfterCooldown()
    {
      yield return new WaitForSeconds(Cooldown);
      SwitchFollowOff();
    }

    private void StopAggroCoroutine()
    {
      if (_aggroCorountine != null)
      {
        StopCoroutine(_aggroCorountine);
        _aggroCorountine = null;
      }
    }

    private void SwitchFollowOn() => 
      Follow.enabled = true;

    private void SwitchFollowOff() => 
      Follow.enabled = false;
  }
}