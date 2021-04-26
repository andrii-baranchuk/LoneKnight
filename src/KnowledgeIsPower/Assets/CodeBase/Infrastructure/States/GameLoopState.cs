using System;

namespace CodeBase.Infrastructure.States
{
  public class GameLoopState : IState
  {
    public GameLoopState(GameStateMachine stateMachine)
    {
      
    }

    public void Exit()
    {
      throw new NotImplementedException();
    }

    public void Enter()
    {
      
    }
  }
}