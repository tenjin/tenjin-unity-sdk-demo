using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        instance.SetAppStoreType(AppStoreType.googleplay);

#if UNITY_IOS
      if (new Version(Device.systemVersion).CompareTo(new Version("14.0")) >= 0) {
        // Tenjin wrapper for requestTrackingAuthorization
        instance.RequestTrackingAuthorizationWithCompletionHandler((status) => {
          Debug.Log("===> App Tracking Transparency Authorization Status: " + status);

          // Sends install/open event to Tenjin
          instance.Connect();

        });
      }
      else {
          instance.Connect();
      }
#elif UNITY_ANDROID

        // Sends install/open event to Tenjin
        instance.Connect();

#endif

    }
}
