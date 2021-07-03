using System;
using UnityEngine;
using UnityEngine.Advertisements;

namespace CodeBase.Infrastructure.Services.Ads
{
  public class AdsService : IUnityAdsListener, IAdsService
  {
    private const string AndroidGameId = "4200887";
    private const string IOSGameId = "4200886";

    private const string AndroidRewardedVideoPlacementId = "Rewarded_Android";
    private const string IOSRewardedVideoPlacementId = "Rewarded_iOS";

    private string _gameId;
    private string _rewardedVideoId;
    private Action _onVideoFinished;

    public event Action RewardedVideoReady;

    public int Reward => 13;


    public void Initialize()
    {
      switch (Application.platform)
      {
        case RuntimePlatform.Android:
          _gameId = AndroidGameId;
          _rewardedVideoId = AndroidRewardedVideoPlacementId;
          break;
        case RuntimePlatform.IPhonePlayer:
          _gameId = IOSGameId;
          _rewardedVideoId = IOSRewardedVideoPlacementId;
          break;
        case RuntimePlatform.WindowsEditor:
          _gameId = IOSGameId;
          _rewardedVideoId = IOSRewardedVideoPlacementId;
          break;
        default:
          Debug.Log("Unsupported platform for ads");
          break;
      }
      
      Advertisement.AddListener(this);
      Advertisement.Initialize(_gameId);
    }

    public void ShowRewardedVideo(Action onVideoFinished)
    {
      Advertisement.Show(_rewardedVideoId);
      _onVideoFinished = onVideoFinished;
    }

    public bool IsRewardedVideoReady => 
      Advertisement.IsReady(_rewardedVideoId);

    public void OnUnityAdsReady(string placementId)
    {
      Debug.Log($"OnUnityAdsReady {placementId}");
      
      if(placementId == _rewardedVideoId)
        RewardedVideoReady?.Invoke();
    }

    public void OnUnityAdsDidError(string message)
    {
      Debug.Log($"OnUnityAdsDidError {message}");
    }

    public void OnUnityAdsDidStart(string placementId)
    {
      Debug.Log($"OnUnityAdsDidStart {placementId}");
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
      switch (showResult)
      {
        case ShowResult.Failed:
          Debug.LogError($"OnUnityAdsDidFinish {showResult}");
          break;
        case ShowResult.Skipped:
          Debug.LogError($"OnUnityAdsDidFinish {showResult}");
          break;
        case ShowResult.Finished:
          _onVideoFinished?.Invoke();
          break;
      }

      _onVideoFinished = null;
    }
  }
}