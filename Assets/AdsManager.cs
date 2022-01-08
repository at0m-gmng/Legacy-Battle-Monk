using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour, IUnityAdsListener
{   
    [SerializeField] private bool _testMode = true;

    private string _gameId = "4500297";
    private string _video = "Interstitial_Android";
    private string _rewardedVideo = "Rewarded_Android";
    private static string _banner = "Banner_Android";
    
    void Start() {
        Advertisement.AddListener(this);
        Advertisement.Initialize(_gameId, _testMode);

        // #region Banner
        // StartCoroutine(ShowBannnerWhenInitialized());
        // Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
        // #endregion
        ShowBanner(true);
    }

    public static void ShowBanner(bool condition) {
        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
        if(condition) {
            Advertisement.Banner.Show(_banner);
        } else {
            Advertisement.Banner.Hide();
        }
    }

    public static void ShowAdsVideo(string placementId) { // инициализация рекламы по типу
        if(Advertisement.IsReady()) {
            Advertisement.Show(placementId);
        } else {
            Debug.Log("not ready");
        }
    }

    // IEnumerator ShowBannnerWhenInitialized() {
    //     while (!Advertisement.isInitialized) {
    //         yield return new WaitForSeconds(.5f);
    //     }
    //     Advertisement.Banner.Show(_banner);
    // }

    public void OnUnityAdsReady(string placementId) {
        // throw new System.NotImplementedException();

        if(placementId == _rewardedVideo) {
            // действия если реклама доступна
        }
    }

    public void OnUnityAdsDidError(string messege) {
        // throw new System.NotImplementedException();

        // ошибка рекламы
    }

    public void OnUnityAdsDidStart(string placementId) {
        // throw new System.NotImplementedException();

        // только запустили рекламу
    }

    //обработка рекламы (определяем вознаграждение)    
    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult) { 
        // throw new System.NotImplementedException();

        if(showResult == ShowResult.Finished) {
            Debug.Log("+ collect / lives");
            // действия, если посмотрели рекламу до конца
        } else if(showResult == ShowResult.Skipped) {
            Debug.Log("skip");
            // действия, если пропустили рекламу
        } else if(showResult == ShowResult.Failed) {
            // действия при ошибке рекламы
        }
    }
}
