using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Windows
{
  public abstract class WindowBase : MonoBehaviour
  {
    public Button CloseButton;

    protected IPersistentProgressService ProgressService;
    protected PlayerProgress Progress => ProgressService.Progress;

    public void Construct(IPersistentProgressService progressService) => 
      ProgressService = progressService;

    private void Awake() =>
      OnAwake();

    protected virtual void OnAwake() =>
      CloseButton.onClick.AddListener(() => Destroy(gameObject));

    private void Start()
    {
      Initialize();
      SubscribeUpdates();
    }

    protected virtual void Initialize(){}

    protected virtual void SubscribeUpdates(){}

    private void OnDestroy()
    {
      CleanUp();
    }

    protected virtual void CleanUp(){}
  }
}