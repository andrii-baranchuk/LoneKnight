using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.StaticData;
using CodeBase.StaticData.Windows;
using CodeBase.UI.Services.Windows;
using CodeBase.UI.Windows;
using UnityEngine;

namespace CodeBase.UI.Services.Factory
{
  class UIFactory : IUIFactory
  {
    private const string UIRootPath = "UI/UIRoot";
    private readonly IAssetProvider _assetsProvider;
    private readonly IStaticDataService _staticData;

    private Transform _uiRoot;
    private readonly IPersistentProgressService _progressService;

    public UIFactory(IAssetProvider assetsProvider, IStaticDataService staticData, IPersistentProgressService progressService)
    {
      _assetsProvider = assetsProvider;
      _staticData = staticData;
      _progressService = progressService;
    }

    public void CreateShop()
    {
      WindowConfig config = _staticData.ForWindow(WindowId.Shop);
      BaseWindow window = Object.Instantiate(config.Prefab, _uiRoot);
      window.Construct(_progressService);
    }

    public void CreateUIRoot() => 
      _uiRoot = _assetsProvider.Instantiate(UIRootPath).transform;
  }
}