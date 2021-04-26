using UnityEngine;

namespace CodeBase.Infrastructure.Input
{
  public class MobileInputService : InputService
  {
    public override Vector2 Axis => SimpleInputAxis();
  }
}