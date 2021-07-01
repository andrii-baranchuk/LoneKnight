using CodeBase.Data;
using TMPro;

namespace CodeBase.UI.Windows
{
  class ShopWindow : BaseWindow
  {
    public TextMeshProUGUI SkullText;
    private LootData LootData => Progress.WorldData.LootData;
    
    protected override void Initialize() => 
      RefreshSkullText();

    protected override void SubscribeUpdate() => 
      LootData.Changed += RefreshSkullText;

    protected override void Cleanup()
    {
      base.Cleanup();
      LootData.Changed -= RefreshSkullText;
    }

    private void RefreshSkullText() => 
      SkullText.text = LootData.Collected.ToString();
  }
}