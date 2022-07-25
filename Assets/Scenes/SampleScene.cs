using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_IOS
using UnityEngine.iOS;
#endif

public class SampleScene : MonoBehaviour
{
    string applovinKey = "T9uaQt-cgbA5tOPtTKv1hdtNehCr8Frfz4M3mPWpBIHD-GGkQ1uwwK2TgBxMgzhaRNb2oeNyLuEzpVFQsT5lku";
#if UNITY_ANDROID && !UNITY_EDITOR
    string _banner = "a7d1aa174c93c716";
    string _interstitial = "9907e304a61c7809";
#elif UNITY_IPHONE && !UNITY_EDITOR
    string _banner = "59b56071d717610f";
    string _interstitial = "9ee0816c54595cc0";
#else
    string _banner = "a7d1aa174c93c716";
    string _interstitial = "9907e304a61c7809";
#endif
    // Start is called before the first frame update
    void Start()
    {
        // Commenting Tenjin Demo app code
        // TenjinConnect();

        // Reproducing customer environment - TENJIN-12301
        InitializeAppLovin();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnApplicationPause(bool pauseStatus)
    {
        if (!pauseStatus)
        {
            // Commenting Tenjin Demo app code
            // TenjinConnect();
        }
    }

// Commenting Tenjin Demo app code
//     public void TenjinConnect()
//     {
//         BaseTenjin instance = Tenjin.getInstance("WZVBMOQEHIMJZD2CZ1VQYIGR16PQAWWC");
// #if UNITY_IOS
//         // instance.RegisterAppForAdNetworkAttribution();
//        float iOSVersion = float.Parse(UnityEngine.iOS.Device.systemVersion);
//        if (iOSVersion == 14.0) {
//             // Tenjin wrapper for requestTrackingAuthorization
//             instance.RequestTrackingAuthorizationWithCompletionHandler((status) => {
//             Debug.Log("===> App Tracking Transparency Authorization Status: " + status);
//             switch (status)
//             {
//                 case 0:
//                     Debug.Log("ATTrackingManagerAuthorizationStatusNotDetermined case");
//                     Debug.Log("Not Determined");
//                     Debug.Log("Unknown consent");
//                     break;
//                 case 1:
//                     Debug.Log("ATTrackingManagerAuthorizationStatusRestricted case");
//                     Debug.Log(@"Restricted");
//                     Debug.Log(@"Device has an MDM solution applied");
//                     break;
//                 case 2: 
//                     Debug.Log("ATTrackingManagerAuthorizationStatusDenied case");
//                     Debug.Log("Denied");
//                     Debug.Log("Denied consent");
//                     break;
//                 case 3:
//                     Debug.Log("ATTrackingManagerAuthorizationStatusAuthorized case");
//                     Debug.Log("Authorized");
//                     Debug.Log("Granted consent");
//                     instance.Connect();
//                     break;
//                 default:
//                     Debug.Log("Unknown");
//                     break;
//             }
//         });
//        } else {
//           instance.Connect();
//       }
// #elif UNITY_ANDROID
//         instance.SetAppStoreType(AppStoreType.googleplay);
//         // Sends install/open event to Tenjin
//         instance.Connect();

// #endif

//     }

        // Reproducing customer environment - TENJIN-12301
        private void TenjinConnect()
        {
            BaseTenjin tenjinInstance = Tenjin.getInstance("YWZKFWDZEREQCFMF3DST3AYHZPCC9MWV");
#if UNITY_ANDROID
            tenjinInstance.SetAppStoreType(AppStoreType.googleplay);
#endif
            tenjinInstance.Connect();
            tenjinInstance.SubscribeAppLovinImpressions();
        }

    private void InitializeAppLovin()
    {
        MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration sdkConfiguration) =>
        {
            Debug.Log($"initialized - sdkConfiguration.ConsentDialogState:{sdkConfiguration.ConsentDialogState}");
#if UNITY_IOS || UNITY_IPHONE || UNITY_EDITOR
            if (MaxSdkUtils.CompareVersions(UnityEngine.iOS.Device.systemVersion, "14.5") != MaxSdkUtils.VersionComparisonResult.Lesser)
            {
                // At Tenjin we don't have audience network.
                // AudienceNetwork.AdSettings.SetAdvertiserTrackingEnabled(sdkConfiguration.AppTrackingStatus == MaxSdkBase.AppTrackingStatus.Authorized);
            }
#endif
                // MMP initialisation is done here as recommended by AppLovin.
                TenjinConnect();
                CreateBanner();
        };
        MaxSdk.SetSdkKey(applovinKey);
        MaxSdk.SetUserId("USER_ID");
        MaxSdk.InitializeSdk();
        ShowBanner();
    }

    public void CreateBanner()
    {
        MaxSdk.CreateBanner(_banner, MaxSdkBase.BannerPosition.TopCenter);
        MaxSdk.SetBannerPlacement(_banner, "First_AdUnit");
        MaxSdk.SetBannerBackgroundColor(_banner, Color.yellow);
    }

    private void OnAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo arg2)
    {
        Debug.Log($"Received impression data - {arg2.Revenue} - {arg2.AdUnitIdentifier} - {arg2.NetworkPlacement}");
    }

    private void ShowBanner()
    {
        MaxSdk.ShowBanner(_banner);
        MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += OnAdRevenuePaidEvent;
    }

}
