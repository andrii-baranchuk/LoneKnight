using CodeBase.Data;
using CodeBase.Infrastructure.Services.Ads;
using CodeBase.Infrastructure.Services.PersistentProgress;
using TMPro;

namespace CodeBase.UI.Windows
{
  class ShopWindow : BaseWindow
  {
    public TextMeshProUGUI SkullText;
    public RewardedAdItem AdItem;
    private LootData LootData => Progress.WorldData.LootData;

    public void Construct(IAdsService adsService, IPersistentProgressService progressService)
    {
      base.Construct(progressService);
      AdItem.Construct(adsService, progressService);
    }

    protected override void Initialize()
    {
      AdItem.Initialize();
      RefreshSkullText();
    }

    protected override void SubscribeUpdate()
    {
      AdItem.Subscribe();
      LootData.Changed += RefreshSkullText;
    }

    protected override void Cleanup()
    {
      base.Cleanup();
      AdItem.Cleanup();
      LootData.Changed -= RefreshSkullText;
    }

    private void RefreshSkullText() => 
      SkullText.text = LootData.Collected.ToString();
  }
}