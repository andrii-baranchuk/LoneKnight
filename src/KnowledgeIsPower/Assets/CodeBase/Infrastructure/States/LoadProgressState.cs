using System;
using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.SaveLoad;

namespace CodeBase.Infrastructure.States
{
  public class LoadProgressState : IState
  {
    private readonly GameStateMachine _stateMachine;
    private readonly IPersistentProgressService _progressService;
    private ISaveLoadService _saveLoadService;

    public LoadProgressState(GameStateMachine stateMachine, IPersistentProgressService progressService, ISaveLoadService saveLoadService)
    {
      _stateMachine = stateMachine;
      _progressService = progressService;
      _saveLoadService = saveLoadService;
    }
    
    public void Enter()
    {
      LoadProgressOrInitNew();
      _stateMachine.Enter<LoadLevelState, string>(_progressService.Progress.WorldData.PositionOnLevel.Level);
    }

    public void Exit()
    {
      
    }

    private void LoadProgressOrInitNew()
    {
      _progressService.Progress = _saveLoadService.LoadProgress() ?? NewProgress();
    }

    private PlayerProgress NewProgress()
    {
      PlayerProgress progress = new PlayerProgress("Main");
      progress.HeroState.MaxHP = 50;
      progress.HeroState.ResetHP();
      progress.HeroStats.Damage = 1f;
      progress.HeroStats.DamageRadius = 0.5f;
      return progress;
    }
  }
}