using System.Collections;
using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic.EnemySpawners;
using TMPro;
using UnityEngine;

namespace CodeBase.Enemy
{
  public class LootPiece : MonoBehaviour, ISavedProgress
  {
    public GameObject Skull;
    public GameObject PickupFxPrefab;
    public TextMeshPro LootText;
    public GameObject PickupPopup;
    
    private Loot _loot;
    private LootPieceData _lootData;
    private bool _picked;

    private WorldData _worldData;
    private SpawnPoint _spawnPoint;

    public void Construct(WorldData worldData)
    {
      _worldData = worldData;
    }

    public void Initialize(Loot loot, SpawnPoint spawnPoint)
    {
      _spawnPoint = spawnPoint;
      _loot = loot;
      _lootData = new LootPieceData(_spawnPoint.Id,_loot, transform.position.AsVectorData());
    }

    private void OnTriggerEnter(Collider other) => Pickup();

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
      
      _worldData.LootData.NotCollectedLoot.RemoveAll(x => x.Id == _lootData.Id);
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

    public void LoadProgress(PlayerProgress progress)
    {
      
    }

    public void UpdateProgress(PlayerProgress progress)
    {
      if (!_picked)
      {
        _worldData.LootData.NotCollectedLoot.Add(_lootData);
      }
    }
  }
}