using UnityEngine;

namespace CodeBase.Infrastructure.Input
{
  public class StandaloneInputService : InputService
  {
    public override Vector2 Axis
    {
      get
      {
        Vector2 axis = SimpleInputAxis();

        if (axis == Vector2.zero)
          axis = UnityAxis();

        return axis;
      }
    }
  }
}