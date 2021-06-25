using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.StaticData;
using CodeBase.StaticData.Windows;
using CodeBase.UI.Services.Windows;
using CodeBase.UI.Windows;
using UnityEngine;

namespace CodeBase.UI.Services.Factory
{
  public class UIFactory : IUIFactory
  {
    private readonly IAssets _assets;
    
    private Transform _uiRoot;
    private readonly IStaticDataService _staticData;
    private readonly IPersistentProgressService _progressService;

    public UIFactory(IAssets assets, IStaticDataService staticData, IPersistentProgressService progressService)
    {
      _assets = assets;
      _staticData = staticData;
      _progressService = progressService;
    }

    public void CreateShop()
    {
      WindowConfig config = _staticData.ForWindow(WindowId.Shop);
      WindowBase window = Object.Instantiate(config.Prefab, _uiRoot);
      window.Construct(_progressService);
    }

    public void CreateUIRoot() => 
      _uiRoot = _assets.Instantiate(AssetPath.UIRootPath).transform;
  }
}