using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services.Ads;
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
    private readonly IAdsService _adsService;
    
    private Transform _uiRoot;
    private readonly IPersistentProgressService _progressService;


    public UIFactory(
      IAssetProvider assetsProvider, 
      IStaticDataService staticData, 
      IPersistentProgressService progressService, 
      IAdsService adsService)
    {
      _assetsProvider = assetsProvider;
      _staticData = staticData;
      _progressService = progressService;
      _adsService = adsService;
    }

    public void CreateShop()
    {
      WindowConfig config = _staticData.ForWindow(WindowId.Shop);
      ShopWindow window = Object.Instantiate(config.Prefab, _uiRoot) as ShopWindow;
      window.Construct(_adsService, _progressService);
    }

    public void CreateUIRoot() => 
      _uiRoot = _assetsProvider.Instantiate(UIRootPath).transform;
  }
}