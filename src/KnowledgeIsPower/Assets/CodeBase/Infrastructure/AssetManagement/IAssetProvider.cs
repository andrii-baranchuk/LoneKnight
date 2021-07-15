using System.Threading.Tasks;
using CodeBase.Infrastructure.Services;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CodeBase.Infrastructure.AssetManagement
{
  public interface IAssetProvider : IService
  {
    void Initialize();
    Task<GameObject> Instantiate(string path);
    Task<GameObject> Instantiate(string path, Vector3 spawnPoint);
    Task<T> Load<T>(AssetReference assetReference) where T : class;
    Task<T> Load<T>(string address) where T : class;
    void CleanUp();
  }
}