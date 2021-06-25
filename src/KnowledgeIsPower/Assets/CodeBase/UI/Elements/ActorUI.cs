using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.UI.Elements
{
  public class ActorUI : MonoBehaviour
  {
    public HpBar HpBar;

    private IHealth _health;

    public void Construct(IHealth heroHealth)
    {
      _health = heroHealth;
      _health.Changed += UpdateHpBar;
    }

    private void Start()
    {
      IHealth health = GetComponent<IHealth>();
      
      if(health != null)
        Construct(health);
    }

    private void OnDestroy()
    {
      if(_health != null)
        _health.Changed -= UpdateHpBar;
    }

    private void UpdateHpBar()
    {
      HpBar.SetValue(_health.Current, _health.Max);
    }
  }
}