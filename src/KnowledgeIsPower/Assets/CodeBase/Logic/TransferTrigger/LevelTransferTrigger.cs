using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.States;
using UnityEngine;

namespace CodeBase.Logic.TransferTrigger
{
  public class LevelTransferTrigger : MonoBehaviour
  {
    private const string PlayerTag = "Player";
    
    public string TransferTo;
    private IGameStateMachine _stateMachine;

    private bool _triggered;

    public void Constuct(IGameStateMachine stateMachine, string transferTo)
    {
      _stateMachine = stateMachine;
      TransferTo = transferTo;
    }
    
    private void OnTriggerEnter(Collider other)
    {
      if (_triggered)
        return;
      
      if (other.CompareTag(PlayerTag))
      {
        _stateMachine.Enter<LoadLevelState, string>(TransferTo);
        _triggered = true;
      }
    }
  }
}