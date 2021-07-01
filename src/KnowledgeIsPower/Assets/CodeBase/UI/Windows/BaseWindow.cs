using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Windows
{
  public abstract class BaseWindow : MonoBehaviour
  {
    public Button CloseButton;
    
    protected IPersistentProgressService ProgressService;
    protected PlayerProgress Progress => ProgressService.Progress;
    
    public void Construct(IPersistentProgressService progressService) => 
      ProgressService = progressService;

    private void Awake() => 
      OnAwake();

    private void Start()
    {
      Initialize();
      SubscribeUpdate();
    }

    private void OnDestroy() => 
      Cleanup();

    protected virtual void OnAwake() => 
      CloseButton.onClick.AddListener(() => Destroy(gameObject));

    protected virtual void Initialize() {}
    protected virtual void SubscribeUpdate() {}
    protected virtual void Cleanup() {}
  }
}