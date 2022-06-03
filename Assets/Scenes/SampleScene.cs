using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_IOS
using UnityEngine.iOS;
#endif

public class SampleScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TenjinConnect();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnApplicationPause(bool pauseStatus)
    {
        if (!pauseStatus)
        {
            TenjinConnect();
        }
    }


    public void TenjinConnect()
    {
        BaseTenjin instance = Tenjin.getInstance("WZVBMOQEHIMJZD2CZ1VQYIGR16PQAWWC");
#if UNITY_IOS
        // instance.RegisterAppForAdNetworkAttribution();
       float iOSVersion = float.Parse(UnityEngine.iOS.Device.systemVersion);
       if (iOSVersion == 14.0) {
            // Tenjin wrapper for requestTrackingAuthorization
            instance.RequestTrackingAuthorizationWithCompletionHandler((status) => {
            Debug.Log("===> App Tracking Transparency Authorization Status: " + status);
            switch (status)
            {
                case 0:
                    Debug.Log("ATTrackingManagerAuthorizationStatusNotDetermined case");
                    Debug.Log("Not Determined");
                    Debug.Log("Unknown consent");
                    break;
                case 1:
                    Debug.Log("ATTrackingManagerAuthorizationStatusRestricted case");
                    Debug.Log(@"Restricted");
                    Debug.Log(@"Device has an MDM solution applied");
                    break;
                case 2: 
                    Debug.Log("ATTrackingManagerAuthorizationStatusDenied case");
                    Debug.Log("Denied");
                    Debug.Log("Denied consent");
                    break;
                case 3:
                    Debug.Log("ATTrackingManagerAuthorizationStatusAuthorized case");
                    Debug.Log("Authorized");
                    Debug.Log("Granted consent");
                    instance.Connect();
                    break;
                default:
                    Debug.Log("Unknown");
                    break;
            }
        });
       } else {
          instance.Connect();
      }
#elif UNITY_ANDROID
        instance.SetAppStoreType(AppStoreType.googleplay);
        // Sends install/open event to Tenjin
        instance.Connect();

#endif

    }
}
