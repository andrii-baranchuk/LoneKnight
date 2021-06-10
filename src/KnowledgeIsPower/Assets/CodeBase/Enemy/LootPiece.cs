using System.Collections;
using CodeBase.Data;
using CodeBase.Logic;
using TMPro;
using UnityEngine;

namespace CodeBase.Enemy
{
  public class LootPiece : MonoBehaviour
  {
    public GameObject Skull;
    public GameObject PickupFxPrefab;
    public TextMeshPro LootText;
    public GameObject PickupPopup;
    public UniqueId Id;

    private Loot _loot;
    private bool _picked;
    private WorldData _worldData;

    public void Construct(WorldData worldData)
    {
      _worldData = worldData;
    }

    public void Initialize(Loot loot)
    {
      _loot = loot;
      transform.position = loot.Position;
      if (IsNewLoot())
      {
        InitLootItem();
      }
      else
      {
        InitLootPiece(loot);
      }
    }

    private void InitLootPiece(Loot loot)
    {
      Id.Id = loot.Id;
    }

    private void InitLootItem()
    {
      _loot.Id = Id.Id;
      _worldData.LootData.UnpickedLoot.Loot.Add(_loot);
    }

    private bool IsNewLoot()
    {
      return _loot.Id == null;
    }

    private void OnTriggerEnter(Collider other) => 
      Pickup();

    private void Pickup()
    {
      if(_picked)
        return;
      
      _picked = true;
      
      UpdateWorldData();
      HideSkull();
      PlayPickupFx();
      ShowText();
      StartCoroutine(StartDestroyTimer());
    }

    private void UpdateWorldData()
    {
      _worldData.LootData.Collect(_loot);
    }
    
    private void HideSkull()
    {
      Skull.SetActive(false);
    }

    private void PlayPickupFx()
    {
      Instantiate(PickupFxPrefab, transform.position, Quaternion.identity);
    }

    private void ShowText()
    {
      LootText.text = $"{_loot.Value}";
      PickupPopup.SetActive(true);
    }

    private IEnumerator StartDestroyTimer()
    {
      yield return new WaitForSeconds(1.5f);
      
      Destroy(gameObject);
    }
  }
}