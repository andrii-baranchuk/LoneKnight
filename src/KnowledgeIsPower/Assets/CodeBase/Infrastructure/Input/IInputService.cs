using CodeBase.Infrastructure.Services;
using UnityEngine;

namespace CodeBase.Infrastructure.Input
{
  public interface IInputService : IService
  {
    Vector2 Axis { get; }

    bool IsAttackButtonUp();
  }
}