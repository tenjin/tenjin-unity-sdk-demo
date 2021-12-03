using System;
using UnityEngine;

public class TenjinMopubIntegration
{
    private static bool _subscribed = false;
    public TenjinMopubIntegration()
    {
    }

    public static void ListenForImpressions(Action<string> callback)
    {
#if tenjin_mopub_enabled
        if (_subscribed)
        {
            Debug.Log("Ignoring duplicate mopub subscription");
            return;
        }
        
        MoPubManager.OnImpressionTrackedEvent += (arg1, arg2) => callback(arg2.JsonRepresentation);
        _subscribed = true;
#endif
    }

}
