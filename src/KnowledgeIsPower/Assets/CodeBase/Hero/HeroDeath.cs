using UnityEngine;

namespace CodeBase.Hero
{
  [RequireComponent(typeof(HeroHealth))]
  public class HeroDeath : MonoBehaviour
  {
    public HeroHealth heroHealth;

    public HeroMove Move;
    public HeroAttack Attack;
    public HeroAnimator Animator;

    public GameObject DeathFx;
    private bool _isDead;


    private void Start() =>
      heroHealth.Changed += HeroHealthChanged;

    private void OnDestroy() => heroHealth.Changed -= HeroHealthChanged;

    private void HeroHealthChanged()
    {
      if (!_isDead && heroHealth.Current <= 0)
        Die();
    }

    private void Die()
    {
      _isDead = true;
      Move.enabled = false;
      Attack.enabled = false;
      Animator.PlayDeath();

      Instantiate(DeathFx, transform.position, Quaternion.identity);
    }
  }
}